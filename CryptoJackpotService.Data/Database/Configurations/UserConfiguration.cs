using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.Email).IsRequired().HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.Password).IsRequired().HasColumnType("text").HasMaxLength(50);
        builder.Property(e => e.Identification).HasColumnType("text").HasMaxLength(50);
        builder.Property(e => e.Phone).HasColumnType("text").HasMaxLength(50);
        builder.Property(e => e.StatePlace).IsRequired().HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.City).IsRequired().HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.Address).HasColumnType("text").HasMaxLength(150);
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.ImagePath).HasColumnType("text").HasMaxLength(200);
        builder.Property(e => e.GoogleAccessToken).HasColumnType("text").HasMaxLength(500);
        builder.Property(e => e.GoogleRefreshToken).HasColumnType("text").HasMaxLength(500);
        builder.Property(e => e.SecurityCode).HasColumnType("text").HasMaxLength(50);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.Phone).IsUnique();
        builder.HasIndex(e => e.SecurityCode).IsUnique();
        builder.HasIndex(e => e.Identification).IsUnique();

        builder.HasOne(e => e.Role).WithMany(r => r.Users).HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Country).WithMany(c => c.Users).HasForeignKey(e => e.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}