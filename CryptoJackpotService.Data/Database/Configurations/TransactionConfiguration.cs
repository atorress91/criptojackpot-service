using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.TransactionNumber).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.Amount).IsRequired().HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.Currency).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(3);
        builder.Property(e => e.Type).IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(e => e.Status).IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(e => e.PaymentMethod).HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.PaymentProvider).HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.ProviderTransactionId).HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.ErrorCode).HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.ErrorMessage).HasColumnType(ColumnTypes.Text).HasMaxLength(500);

        builder.HasIndex(e => e.TransactionNumber).IsUnique();
        builder.HasIndex(e => e.ProviderTransactionId);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Invoice)
            .WithOne(e => e.Transaction)
            .HasForeignKey<Transaction>(e => e.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}