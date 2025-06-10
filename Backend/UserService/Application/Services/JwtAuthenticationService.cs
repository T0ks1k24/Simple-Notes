using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Domain.Models.LoginApi;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtAuthenticationService
{
     private readonly IConfiguration _configuration;
     private readonly IUserRepository _userRepository;
     private readonly IRefreshTokenRepository _refreshTokenRepository;

     public JwtAuthenticationService(IConfiguration configuration, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
     {
          _configuration = configuration;
          _userRepository = userRepository;
          _refreshTokenRepository = refreshTokenRepository;
     }

     public async Task<LoginResponseModel?> Authenticate(LoginRequestModel request)
     {
          if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
               throw new ApplicationException("The email or password is invalid.");
          
          var user = await _userRepository.GetUserByEmail(request.Email);
          if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password!))
               throw new ApplicationException("The email or password is invalid.");
          
          return await GenerateJwtToken(user);
     }

     private async Task<LoginResponseModel> GenerateJwtToken(User user)
     {
          var issuer = _configuration["JwtConfig:Issuer"];
          var audience = _configuration["JwtConfig:Audience"];
          var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]);
          var tokenValidityHours = int.Parse(_configuration["JwtConfig:TokenValidityHours"]);
          var tokenExpiryTimeStamp = DateTime.UtcNow.AddHours(tokenValidityHours);

          var token = new JwtSecurityToken(issuer, audience, [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("name", user.Username),
                    new Claim("role", user.Role),
               ],
               expires: tokenExpiryTimeStamp,
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256));
          
          var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
          return new LoginResponseModel
          {
               Username = user.Username,
               AccessToken = accessToken,
               ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
               RefreshToken = await GenerateRefreshToken(user.Id)
          };
     }
     
     public async Task<LoginResponseModel?> ValidateRefreshToken(string token)
     {
          var refreshToken = await _refreshTokenRepository.GetValidRefreshToken(token);
          if (refreshToken is null || refreshToken.Expiry < DateTime.UtcNow)
          {
               return null;
          }

          await _refreshTokenRepository.DeleteRefreshToken(refreshToken.Id);
          
          var user = await _userRepository.GetUserById(refreshToken.UserId);
          if (user is null) return null;

          return await GenerateJwtToken(user);
     }
     
     private async Task<string> GenerateRefreshToken(Guid userId)
     {
          var refreshTokenValidityDays = int.Parse(_configuration["JwtConfig:RefreshTokenValidityDays"]);
          var refreshToken = new RefreshToken
          {
               Id = Guid.NewGuid(),
               Token = Guid.NewGuid().ToString(),
               Expiry = DateTime.UtcNow.AddDays(refreshTokenValidityDays),
               UserId = userId,
          }; 
          await _refreshTokenRepository.DeleteRefreshTokenByUserId(refreshToken.UserId);
          await _refreshTokenRepository.CreateRefreshToken(refreshToken);
          
          return refreshToken.Token;
     }
}