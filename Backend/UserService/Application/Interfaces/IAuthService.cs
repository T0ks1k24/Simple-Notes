using Domain.Models.JwtTokenApi;
using Domain.Models.LoginApi;
using Domain.Models.RegisterApi;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<bool> CreateUserAsync(RegisterRequestApi register);
    Task<JwtToken> LoginAsync(LoginRequestModel login);
}