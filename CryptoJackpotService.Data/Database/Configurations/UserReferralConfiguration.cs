using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class UserReferralConfiguration : IEntityTypeConfiguration<UserReferral>
{
    public void Configure(EntityTypeBuilder<UserReferral> builder)
    {
        builder.ToTable("user_referral");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UsedSecurityCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        
        // Relación: Un Usuario (Referrer) puede tener muchos referidos
        builder.HasOne(x => x.Referrer)
            .WithMany(u => u.Referrals)
            .HasForeignKey(x => x.ReferrerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación: Un Usuario puede ser referido solo una vez
        builder.HasOne(x => x.Referred)
            .WithOne(u => u.ReferredBy)
            .HasForeignKey<UserReferral>(x => x.ReferredId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices para optimizar consultas
        builder.HasIndex(x => x.ReferrerId);
        builder.HasIndex(x => x.ReferredId).IsUnique(); // Un usuario solo puede ser referido una vez
        builder.HasIndex(x => x.UsedSecurityCode);
        
        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}