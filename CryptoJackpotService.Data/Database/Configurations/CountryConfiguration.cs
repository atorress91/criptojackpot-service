using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class CountryConfiguration:IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.Iso3).HasColumnType("text").HasMaxLength(3);
        builder.Property(e => e.NumericCode).HasColumnType("text").HasMaxLength(3);
        builder.Property(e => e.Iso2).HasColumnType("text").HasMaxLength(2);
        builder.Property(e => e.PhoneCode).HasColumnType("text").HasMaxLength(20);
        builder.Property(e => e.Capital).HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.Currency).HasColumnType("text").HasMaxLength(3);
        builder.Property(e => e.CurrencyName).HasColumnType("text").HasMaxLength(50);
        builder.Property(e => e.CurrencySymbol).HasColumnType("text").HasMaxLength(5);
        builder.Property(e => e.Tld).HasColumnType("text").HasMaxLength(10);
        builder.Property(e => e.Native).HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.Region).HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.Subregion).HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.Latitude).HasColumnType("decimal(10,8)");
        builder.Property(e => e.Longitude).HasColumnType("decimal(11,8)");
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
            
        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}