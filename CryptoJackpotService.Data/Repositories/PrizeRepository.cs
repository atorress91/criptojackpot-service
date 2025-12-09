using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;

namespace CryptoJackpotService.Data.Repositories;

public class PrizeRepository(CryptoJackpotDbContext context) : BaseRepository(context), IPrizeRepository
{
    public async Task<Prize> CreatePrizeAsync(Prize prize)
    {
        var today = DateTime.UtcNow;
        prize.CreatedAt = today;
        prize.UpdatedAt = today;

        await Context.Prizes.AddAsync(prize);
        await Context.SaveChangesAsync();

        return prize;
    }
}