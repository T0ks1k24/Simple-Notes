using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateRefreshToken(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetValidRefreshToken(string token) => 
        await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);
    
    public async Task UpdateRefreshToken(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenById(Guid id) => 
        await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Id == id);
    
    public async Task<RefreshToken?> GetRefreshTokenByUserId(Guid userId) => 
        await _context.RefreshTokens.FirstOrDefaultAsync(r => r.UserId == userId);

    public async Task DeleteRefreshToken(Guid id)
    {
        var token = await GetRefreshTokenById(id);
        if (token != null)
        {
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteRefreshTokenByUserId(Guid userId)
    {
        var token = await GetRefreshTokenByUserId(userId);
        if (token != null)
        {
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }
}