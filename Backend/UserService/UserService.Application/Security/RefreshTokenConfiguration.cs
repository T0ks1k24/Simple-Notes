using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Application.Models;

namespace UserService.Application.Security;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Token).HasMaxLength(200);
        
        builder.HasIndex(x => x.Token).IsUnique();
        
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
    }
}