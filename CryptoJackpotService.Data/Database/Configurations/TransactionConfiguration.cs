using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.TransactionNumber).IsRequired().HasColumnType("text").HasMaxLength(50);
        builder.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.Currency).IsRequired().HasColumnType("text").HasMaxLength(3);
        builder.Property(e => e.Type).IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(e => e.Status).IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(e => e.PaymentMethod).HasColumnType("text").HasMaxLength(50);
        builder.Property(e => e.PaymentProvider).HasColumnType("text").HasMaxLength(50);
        builder.Property(e => e.ProviderTransactionId).HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.ErrorCode).HasColumnType("text").HasMaxLength(50);
        builder.Property(e => e.ErrorMessage).HasColumnType("text").HasMaxLength(500);

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