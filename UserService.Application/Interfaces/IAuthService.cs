using UserService.Application.Models;
using UserService.Application.Models.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IAuthService
{
    Task<bool> CreateUserAsync(RegisterDTO register);
    Task<AuthenticateDTO> AuthenticateAsync(LoginDTO login);
}