using CryptoJackpotService.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace CryptoJackpotService.Tests.Infrastructure;

public class TestCryptoJackpotDbContext : CryptoJackpotDbContext
{
    public TestCryptoJackpotDbContext(DbContextOptions<CryptoJackpotDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Ignore properties that are not supported by the InMemory provider
        modelBuilder.Entity<CryptoJackpotService.Data.Database.Models.Prize>()
            .Ignore(p => p.Specifications);
    }
}
