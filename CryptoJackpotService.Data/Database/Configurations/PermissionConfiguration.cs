using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.Description).HasColumnType("text").HasMaxLength(200);
        builder.Property(e => e.Module).IsRequired().HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
            
        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}