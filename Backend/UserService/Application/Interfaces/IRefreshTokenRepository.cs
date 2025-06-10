using Domain.Entities;

namespace Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task CreateRefreshToken(RefreshToken refreshToken);
    Task UpdateRefreshToken(RefreshToken refreshToken);
    Task<RefreshToken?> GetValidRefreshToken(string token);
    Task<RefreshToken?> GetRefreshTokenById(Guid id);
    Task<RefreshToken?> GetRefreshTokenByUserId(Guid userId);
    Task DeleteRefreshToken(Guid id);
    Task DeleteRefreshTokenByUserId(Guid userId);
    
}