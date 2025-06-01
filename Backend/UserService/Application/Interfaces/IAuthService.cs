using Domain.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<bool> CreateUserAsync(RegisterDTO register);
    // Task<AuthenticateDTO> AuthenticateAsync(LoginDTO login);
}