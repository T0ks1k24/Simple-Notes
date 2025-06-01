using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Login;

namespace Application.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public JwtService(IConfiguration configuration, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResponseModel?> Authenticate(LoginRequestModel request)
    {
        if(string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new ApplicationException("The email or password is invalid.");
        
        var user = await _userRepository.GetByEmail(request.Email);
        if (user == null)
            throw new ApplicationException("The email or password is invalid.");
        
        bool checkPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        
        if (!checkPassword)
            throw new ApplicationException("The email or password is invalid.");
    }
    
    
    public sealed record Response(string AccessToken, string RefreshToken);
    
    public async Task<string> CreateAccessTokenFromUser(string email, string password)
    {
        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtConfig:Key"]));
        var now = DateTime.UtcNow;
        var tokenValidatyDays = Convert.ToInt32(_configuration["JwtConfig:TokenValidityDays"]);
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddHours(tokenValidatyDays);
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, password),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, user.Role),
            }),
            Issuer = issuer,
            Audience = audience,
            NotBefore = now,
            Expires = tokenExpiryTimeStamp,
            IssuedAt = now,   
            SigningCredentials = signingCredentials,
        };
        
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    public async Task<Response> RefreshToken(string token)
    {
        var existingRefreshToken = await _refreshTokenRepository.GetValidRefreshToken(token);
        if(existingRefreshToken is null)
            throw new ApplicationException("The refresh token is invalid or expired.");
        
        var user = existingRefreshToken.User;
        string accessToken = await CreateAccessTokenFromUser(user);
        
        existingRefreshToken.Token = GenerateRefreshToken();
        existingRefreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);
        await _refreshTokenRepository.UpdateRefreshToken(existingRefreshToken);
        
        return new Response(accessToken, existingRefreshToken.Token);
    }
    
    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
    
}