using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class LotteryConfiguration : IEntityTypeConfiguration<Lottery>
{
    public void Configure(EntityTypeBuilder<Lottery> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.LotteryNo).IsRequired().HasColumnType("text").HasMaxLength(50);
        builder.Property(e => e.Title).IsRequired().HasColumnType("text").HasMaxLength(200);
        builder.Property(e => e.Description).IsRequired().HasColumnType("text").HasMaxLength(500);
        builder.Property(e => e.MinNumber).IsRequired();
        builder.Property(e => e.MaxNumber).IsRequired();
        builder.Property(e => e.TotalSeries).IsRequired();
        builder.Property(e => e.TicketPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.MaxTickets).IsRequired();
        builder.Property(e => e.SoldTickets).IsRequired();
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.EndDate).IsRequired();
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.Type).IsRequired();
        builder.Property(e => e.Terms).IsRequired().HasColumnType("text");
        builder.Property(e => e.HasAgeRestriction).IsRequired();
        builder.Property(e => e.MinimumAge);
        builder.Property(e => e.RestrictedCountries).HasColumnType("jsonb");
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasIndex(e => e.LotteryNo).IsUnique();
        
        builder.HasMany(e => e.PrizeTiers)
            .WithOne(e => e.Lottery)
            .HasForeignKey(e => e.LotteryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Tickets)
            .WithOne(e => e.Lottery)
            .HasForeignKey(e => e.LotteryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}