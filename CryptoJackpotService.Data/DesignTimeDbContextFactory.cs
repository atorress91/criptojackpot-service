using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CryptoJackpotService.Data.Database;

namespace CryptoJackpotService.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CryptoJackpotDbContext>
{
    public CryptoJackpotDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CryptoJackpotDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=crypto-jackpot;Username=postgres;Password=123456789");

        return new CryptoJackpotDbContext(optionsBuilder.Options);
    }
}