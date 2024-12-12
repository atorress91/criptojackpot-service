using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoJackpotService.Data.Database;

public class CryptoJackpotDbContext(DbContextOptions<CryptoJackpotDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<RolePermission> RolePermissions { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     => optionsBuilder.UseSnakeCaseNamingConvention();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.Password).IsRequired().HasColumnType("text").HasMaxLength(50);
            entity.Property(e => e.Identification).HasColumnType("text").HasMaxLength(50);
            entity.Property(e => e.Phone).HasColumnType("text").HasMaxLength(50);
            entity.Property(e => e.StatePlace).IsRequired().HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.City).IsRequired().HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.Address).HasColumnType("text").HasMaxLength(150);
            entity.Property(e=>e.Status).IsRequired();
            entity.Property(e => e.ImagePath).HasColumnType("text").HasMaxLength(200);
            entity.Property(e => e.GoogleAccessToken).HasColumnType("text").HasMaxLength(500);
            entity.Property(e => e.GoogleRefreshToken).HasColumnType("text").HasMaxLength(500);

            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(e => e.Role).WithMany(r => r.Users).HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Country).WithMany(c => c.Users).HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });
        
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnType("text").HasMaxLength(200);

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnType("text").HasMaxLength(200);
            entity.Property(e => e.Module).IsRequired().HasColumnType("text").HasMaxLength(100);

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId });
            entity.Property(e => e.AccessLevel).IsRequired();

            entity.HasOne(e => e.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });
        
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.Iso3).HasColumnType("text").HasMaxLength(3);
            entity.Property(e => e.NumericCode).HasColumnType("text").HasMaxLength(3);
            entity.Property(e => e.Iso2).HasColumnType("text").HasMaxLength(2);
            entity.Property(e => e.PhoneCode).HasColumnType("text").HasMaxLength(20);
            entity.Property(e => e.Capital).HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.Currency).HasColumnType("text").HasMaxLength(3);
            entity.Property(e => e.CurrencyName).HasColumnType("text").HasMaxLength(50);
            entity.Property(e => e.CurrencySymbol).HasColumnType("text").HasMaxLength(5);
            entity.Property(e => e.Tld).HasColumnType("text").HasMaxLength(10);
            entity.Property(e => e.Native).HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.Region).HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.Subregion).HasColumnType("text").HasMaxLength(100);
            entity.Property(e => e.Latitude).HasColumnType("decimal(10,8)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(11,8)");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });
    }
}