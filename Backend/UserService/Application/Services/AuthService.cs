using Application.Interfaces;
using Domain.Entities;
using Domain.Models.JwtTokenApi;
using Domain.Models.LoginApi;
using Domain.Models.RegisterApi;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtAuthenticationService _jwtAuthenticationService;
    public AuthService(IUserRepository userRepository, JwtAuthenticationService jwtAuthenticationService)
    {
        _userRepository = userRepository;
        _jwtAuthenticationService = jwtAuthenticationService;
    }
    
    //Method Register User And Verify Name, Email And Hash Password
    public async Task<bool> CreateUserAsync(RegisterRequestApi register)
    {
        if (string.IsNullOrWhiteSpace(register.Name) || string.IsNullOrWhiteSpace(register.Email) || string.IsNullOrWhiteSpace(register.Password))
            throw new ApplicationException("The name, email or password is invalid.");
        
        var existingUser = await _userRepository.GetUserByEmail(register.Email);
        if (existingUser != null)
            throw new ApplicationException("The email is already taken.");
            
        
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);
        
        var addUser = new User
        {
            Username = register.Name,
            Email = register.Email,
            Password = hashedPassword,
        };

        return await _userRepository.CreateUser(addUser);
    }
    
    public async Task<JwtToken> LoginAsync(LoginRequestModel login)
    {
        var response = await _jwtAuthenticationService.Authenticate(login);
        
        return new JwtToken
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken
        };
    }
}
