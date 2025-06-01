using Domain.Models;

namespace Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task CreateRefreshToken(RefreshToken refreshToken);
    Task<RefreshToken?> GetValidRefreshToken(string token);
    Task UpdateRefreshToken(RefreshToken refreshToken);
}