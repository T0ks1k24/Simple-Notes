using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Application.Models.DTOs;

namespace UserService.Application.Security;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public JwtService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    //Method Authenticate User And Return Token
    public async Task<AuthenticateDTO> Authenticate(LoginDTO login)
    {
        if(string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            return null;
        
        var userAccount = await _userRepository.GetByEmail(login.Email);

        if (userAccount is null || !BCrypt.Net.BCrypt.Verify(login.Password, userAccount.Password))
            return null;
        
        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = _configuration["JwtConfig:Key"];
        var tokenValidatyDays = int.Parse(_configuration["JwtConfig:TokenValidityDays"]);
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddDays(tokenValidatyDays);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, login.Email)
            }),
            Expires = tokenExpiryTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature),
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return new AuthenticateDTO
        {
            Name = userAccount.Name,
            Token = accessToken,
            ExpiresIn = tokenValidatyDays
        };
    }
}