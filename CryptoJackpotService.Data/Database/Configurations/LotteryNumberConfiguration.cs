using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class LotteryNumberConfiguration:IEntityTypeConfiguration<LotteryNumber>
{
    public void Configure(EntityTypeBuilder<LotteryNumber> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.LotteryId).IsRequired();
        builder.Property(e => e.Number).IsRequired();
        builder.Property(e => e.Series).IsRequired();
        builder.Property(e => e.IsAvailable).IsRequired();
        builder.Property(e => e.TicketId);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasOne(e => e.Ticket)
            .WithMany(e => e.SelectedNumbers)
            .HasForeignKey(e => e.TicketId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}