using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Application.Interfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public JwtService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public sealed record Response(string AccessToken, string RefreshToken);
    
    //Method Authenticate User And Return Token
    public async Task<Response> Authenticate(LoginDTO login)
    {
        if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            return null;
        
        var userAccount = await _userRepository.GetByEmail(login.Email);

        if (userAccount is null || !BCrypt.Net.BCrypt.Verify(login.Password, userAccount.Password))
            return null;
        
        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtConfig:Key"]));
        var tokenValidatyDays = int.Parse(_configuration["JwtConfig:TokenValidityDays"]);
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddHours(tokenValidatyDays);
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, userAccount.Name),
                new Claim(JwtRegisteredClaimNames.Email, userAccount.Email),
                new Claim(ClaimTypes.Role, userAccount.Role),
            }),
            Expires = tokenExpiryTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = signingCredentials,
        };
        
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        var accessToken = handler.WriteToken(token);
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userAccount.Id,
            Token = GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        return new Response(accessToken, refreshToken.Token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}