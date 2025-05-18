using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Models;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetByName(string namePart)
    {
        return await _dbContext.Users
            .Where(u => u.Name.ToLower()
            .Contains(namePart.ToLower()))
            .ToListAsync();
    }

    public async Task<bool> CreateUser(User user)
    {
        await _dbContext.Users.AddAsync(user);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<ViewUser> GetById(Guid id)
    { 
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        return new ViewUser
        {
            Name = user?.Name,
            Email = user?.Email,
            Role = user?.Role
        };
    }

    public async Task<List<User>> GetAll()
    {
        return await _dbContext.Users.AsNoTracking().ToListAsync();
    }
}