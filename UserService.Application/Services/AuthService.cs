using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        var addUser = new User
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
        };

        return await _userRepository.CreateUser(addUser);
    }

    public async Task<UserDTO> AuthenticateAsync(string email, string password)
    {
        
    }
}