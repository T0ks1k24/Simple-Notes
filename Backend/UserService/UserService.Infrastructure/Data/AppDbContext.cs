using Microsoft.EntityFrameworkCore;
using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data;

//Database application setting db 
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    //DbSet Table
    public DbSet<User> Users { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}