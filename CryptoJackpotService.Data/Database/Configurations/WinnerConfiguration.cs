using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class WinnerConfiguration : IEntityTypeConfiguration<Winner>
{
    public void Configure(EntityTypeBuilder<Winner> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.WinDate).IsRequired();
        builder.Property(e => e.ClaimDate);
        builder.Property(e => e.DeliveryAddress).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(500);
        builder.Property(e => e.DeliveryStatus).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.HasSelectedCashAlternative).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasOne(e => e.Lottery)
            .WithMany()
            .HasForeignKey(e => e.LotteryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Ticket)
            .WithMany()
            .HasForeignKey(e => e.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PrizeTier)
            .WithMany()
            .HasForeignKey(e => e.PrizeTierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}