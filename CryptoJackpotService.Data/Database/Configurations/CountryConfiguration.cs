using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class CountryConfiguration:IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.Iso3).HasColumnType(ColumnTypes.Text).HasMaxLength(3);
        builder.Property(e => e.NumericCode).HasColumnType(ColumnTypes.Text).HasMaxLength(3);
        builder.Property(e => e.Iso2).HasColumnType(ColumnTypes.Text).HasMaxLength(2);
        builder.Property(e => e.PhoneCode).HasColumnType(ColumnTypes.Text).HasMaxLength(20);
        builder.Property(e => e.Capital).HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.Currency).HasColumnType(ColumnTypes.Text).HasMaxLength(3);
        builder.Property(e => e.CurrencyName).HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.CurrencySymbol).HasColumnType(ColumnTypes.Text).HasMaxLength(5);
        builder.Property(e => e.Tld).HasColumnType(ColumnTypes.Text).HasMaxLength(10);
        builder.Property(e => e.Native).HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.Region).HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.Subregion).HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.Latitude).HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.Longitude).HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
            
        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}