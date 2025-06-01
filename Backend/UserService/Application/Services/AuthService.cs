using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    //Method Register User And Verify Name, Email And Hash Password
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
}