using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    //Method Get All User For Admin
    public async Task<List<User>> GetAllUser() =>
        await _context.Users.AsNoTracking().ToListAsync();
    
    //Method Get By Id On Information User
    public async Task<User> GetUserById(Guid id) => 
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    
    //Method Get By Email On Info User
    public async Task<User?> GetUserByEmail(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    //Method Get By Name In User List Information
    public async Task<List<User>> GetUserByName(string namePart)
    {
        return await _context.Users
            .Where(u => u.Username.ToLower()
                .Contains(namePart.ToLower()))
            .ToListAsync();
    }

    //Method Create User 
    public async Task<bool> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        return await _context.SaveChangesAsync() > 0;
    }
    
    //Method Update User 
    public async Task<bool> UpdateUser(User user)
    {
        _context.Users.Update(user);
        return await _context.SaveChangesAsync() > 0;
    }

    //Method Delete User 
    public async Task DeleteUser(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }


    
    
   
    
    
}