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

     public JwtAuthenticationService(IConfiguration configuration, IUserRepository userRepository)
     {
          _configuration = configuration;
          _userRepository = userRepository;
     }

     public async Task<LoginResponseModel?> Authenticate(LoginRequestModel request)
     {
          if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
               return null;
          
          var user = await _userRepository.GetByEmail(request.Email);
          if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password!))
               return null;
          
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
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
               ],
               expires: tokenExpiryTimeStamp,
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature));
          
          var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
          return new LoginResponseModel
          {
               Username = user.Username,
               AccessToken = accessToken,
               ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
          };
     }

     private async Task<string> GenerateRefreshToken(int userId)
     {
          var refreshTokenValidityHours = int.Parse(_configuration["JwtConfig:RefreshTokenValidityHours"]);
          var refreshToken = new RefreshToken
          {
               Token = Guid.NewGuid().ToString(),
               Expiry = DateTime.UtcNow.AddMinutes(refreshTokenValidityHours)
               UserId = userId
          };
          await _refreshTokenRepository.Create(refreshToken);
          
          return refreshToken.Token;

     }
}