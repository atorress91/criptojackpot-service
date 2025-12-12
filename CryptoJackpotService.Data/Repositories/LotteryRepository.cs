using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
namespace CryptoJackpotService.Data.Repositories;

public class LotteryRepository : BaseRepository, ILotteryRepository
{
    protected LotteryRepository(CryptoJackpotDbContext context) : base(context)
    {
    }

    public async Task<Lottery> CreateAsync(Lottery lottery)
    {
        var today = DateTime.Now;

        lottery.CreatedAt = today;
        lottery.UpdatedAt = today;

        await Context.Lotteries.AddAsync(lottery);
        await Context.SaveChangesAsync();

        return lottery;
    }
}