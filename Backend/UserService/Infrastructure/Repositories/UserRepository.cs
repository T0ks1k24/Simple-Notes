using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //Method Get By Email On Info User
    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    //Method Get By Name In User List Information
    public async Task<List<User>> GetByName(string namePart)
    {
        return await _dbContext.Users
            .Where(u => u.Name.ToLower()
                .Contains(namePart.ToLower()))
            .ToListAsync();
    }

    //Method Create User 
    public async Task<bool> CreateUser(User user)
    {
        await _dbContext.Users.AddAsync(user);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    //Method Get By Id On Information User
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

    //Method Get All User For Admin
    public async Task<List<User>> GetAll()
    {
        return await _dbContext.Users.AsNoTracking().ToListAsync();
    }
}