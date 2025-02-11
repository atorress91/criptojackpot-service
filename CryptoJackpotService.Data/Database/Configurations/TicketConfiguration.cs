using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.PurchaseAmount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.PurchaseDate).IsRequired();
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.TransactionId).IsRequired().HasColumnType("text").HasMaxLength(100);
        builder.Property(e => e.IsGift).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasIndex(e => e.TransactionId).IsUnique();
        builder.HasIndex(e => new {e.LotteryId,e.UserId,e.PurchaseDate});
        
        builder.HasOne(e => e.Lottery)
            .WithMany(e => e.Tickets)
            .HasForeignKey(e => e.LotteryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.GiftRecipient)
            .WithMany()
            .HasForeignKey(e => e.GiftRecipientId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.SelectedNumbers)
            .WithOne(e => e.Ticket)
            .HasForeignKey(e => e.TicketId);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}