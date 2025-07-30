﻿using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoJackpotService.Data.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.Email).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.Password).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.Identification).HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.Phone).HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.StatePlace).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.City).IsRequired().HasColumnType(ColumnTypes.Text).HasMaxLength(100);
        builder.Property(e => e.Address).HasColumnType(ColumnTypes.Text).HasMaxLength(150);
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.ImagePath).HasColumnType(ColumnTypes.Text).HasMaxLength(200);
        builder.Property(e => e.GoogleAccessToken).HasColumnType(ColumnTypes.Text).HasMaxLength(500);
        builder.Property(e => e.GoogleRefreshToken).HasColumnType(ColumnTypes.Text).HasMaxLength(500);
        builder.Property(e => e.SecurityCode).HasColumnType(ColumnTypes.Text).HasMaxLength(50);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.SecurityCode).IsUnique();

        builder.HasOne(e => e.Role).WithMany(r => r.Users).HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Country).WithMany(c => c.Users).HasForeignKey(e => e.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Navigation(u=>u.Role).AutoInclude();

        builder.HasQueryFilter(e => !e.DeletedAt.HasValue);
    }
}