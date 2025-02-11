using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class PrizeImageConfiguration:IEntityTypeConfiguration<PrizeImage>
{
    public void Configure(EntityTypeBuilder<PrizeImage> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ImageUrl).IsRequired().HasColumnType("text").HasMaxLength(500);
        builder.Property(e => e.Caption).IsRequired().HasColumnType("text").HasMaxLength(200);
        builder.Property(e => e.DisplayOrder).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasOne(e => e.Prize)
            .WithMany()
            .HasForeignKey(e => e.PrizeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue); 
    }
}