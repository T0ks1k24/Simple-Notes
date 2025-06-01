using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;
    public AuthService(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }
    
    public sealed record JwtResponse(string accessToken, string refreshToken);

    //Method Register User And Verify Name, Email And Hash Password
    public async Task<bool> CreateUserAsync(RegisterDTO register)
    {
        if (string.IsNullOrWhiteSpace(register.Name) || string.IsNullOrWhiteSpace(register.Email) || string.IsNullOrWhiteSpace(register.Password))
            throw new ApplicationException("The name, email or password is invalid.");
        
        var existingUser = await _userRepository.GetByEmail(register.Email);
        if (existingUser != null)
            throw new ApplicationException("The email is already taken.");
            
        
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);
        
        var addUser = new User
        {
            Name = register.Name,
            Email = register.Email,
            Password = hashedPassword,
        };

        return await _userRepository.CreateUser(addUser);
    }
    
    public async Task<JwtResponse> LoginAsync(LoginDTO login)
    {
        if(string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            throw new ApplicationException("The email or password is invalid.");
        
        var user = await _userRepository.GetByEmail(login.Email);
        if (user == null)
            throw new ApplicationException("The email or password is invalid.");
        
        if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            throw new ApplicationException("The email or password is invalid.");

        var accessToken = _jwtService.CreateAccessTokenFromUser(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        
        return new JwtResponse(await accessToken, refreshToken);
    }
}
