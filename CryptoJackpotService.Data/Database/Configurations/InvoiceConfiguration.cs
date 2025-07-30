using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.InvoiceNumber).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.InvoiceDate).IsRequired();
        builder.Property(e => e.SubTotal).IsRequired().HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.Tax).IsRequired().HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.Total).IsRequired().HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.Notes).HasColumnType(ColumnTypes.Text).HasMaxLength(500);
        builder.Property(e => e.Status).IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(e => e.InvoiceNumber).IsUnique();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Details)
            .WithOne(e => e.Invoice)
            .HasForeignKey(e => e.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}