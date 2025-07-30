using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
{
    public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.UnitPrice).IsRequired().HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.Quantity).IsRequired();
        builder.Property(e => e.SubTotal).IsRequired().HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.Tax).IsRequired().HasColumnType(ColumnTypes.Decimal);
        builder.Property(e => e.Total).IsRequired().HasColumnType(ColumnTypes.Decimal);

        builder.HasOne(e => e.Invoice)
            .WithMany(e => e.Details)
            .HasForeignKey(e => e.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Ticket)
            .WithMany()
            .HasForeignKey(e => e.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}