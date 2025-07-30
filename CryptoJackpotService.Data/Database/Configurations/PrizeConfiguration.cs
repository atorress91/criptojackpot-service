using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class PrizeConfiguration : IEntityTypeConfiguration<Prize>
{
    public void Configure(EntityTypeBuilder<Prize> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(200);
        builder.Property(e => e.Description).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(500);
        builder.Property(e => e.EstimatedValue).IsRequired().HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.Type).IsRequired();
        builder.Property(e => e.MainImageUrl).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(500);
        builder.Property(e => e.Specifications).HasColumnType(ColumnTypes.Jsonb);
        builder.Property(e => e.CashAlternative).HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.IsDeliverable).IsRequired();
        builder.Property(e => e.IsDigital).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
        
        builder.HasMany(e=>e.AdditionalImages)
            .WithOne(e=>e.Prize)
            .HasForeignKey(e=>e.PrizeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}