using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Application.Models.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> CreateUserAsync(RegisterDTO register)
    {
        if (string.IsNullOrWhiteSpace(register.Name) || string.IsNullOrWhiteSpace(register.Email) || string.IsNullOrWhiteSpace(register.Password))
            return false;
        
        string HashPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);
        var addUser = new User
        {
            Name = register.Name,
            Email = register.Email,
            Password = HashPassword,
        };

        return await _userRepository.CreateUser(addUser);
    }

    public async Task<AuthenticateDTO?> AuthenticateAsync(LoginDTO login)
    {
        var user = await _userRepository.GetByEmail(login.Email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            return null;
        
        return new AuthenticateDTO
        {
            Name = user.Name,
            Token = "Beaver: token"
        };
    }
}