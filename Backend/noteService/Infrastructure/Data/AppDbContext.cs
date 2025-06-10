using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

//Database application setting db 
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    //DbSet Table
    public DbSet<Note> Notes { get; set; }
    public DbSet<NoteList> NoteLists { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}