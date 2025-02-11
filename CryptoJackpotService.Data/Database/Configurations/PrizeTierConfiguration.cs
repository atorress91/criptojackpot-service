using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class PrizeTierConfiguration:IEntityTypeConfiguration<PrizeTier>
{
    public void Configure(EntityTypeBuilder<PrizeTier> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Tier).IsRequired();
        builder.Property(e => e.NumberOfWinners).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasOne(e => e.Lottery)
            .WithMany(e => e.PrizeTiers)
            .HasForeignKey(e => e.LotteryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Prize)
            .WithMany()
            .HasForeignKey(e => e.PrizeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}