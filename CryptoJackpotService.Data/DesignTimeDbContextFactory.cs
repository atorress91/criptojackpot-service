using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CryptoJackpotService.Data.Database;

namespace CryptoJackpotService.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CryptoJackpotDbContext>
{
    public CryptoJackpotDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CryptoJackpotDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=crypto-jackpot;Username=postgres;Password=postgres123");

        return new CryptoJackpotDbContext(optionsBuilder.Options);
    }
}