using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Custom;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CryptoJackpotService.Data.Repositories;

public class LotteryRepository(
    CryptoJackpotDbContext context) : BaseRepository(context), ILotteryRepository
{
    public async Task<Lottery> CreateAsync(Lottery lottery)
    {
        var today = DateTime.Now;

        lottery.CreatedAt = today;
        lottery.UpdatedAt = today;

        await Context.Lotteries.AddAsync(lottery);
        await Context.SaveChangesAsync();

        return lottery;
    }

    public async Task<PagedList<Lottery>> GetAllLotteriesAsync(Pagination pagination)
    {
        var totalItems = await Context.Lotteries.CountAsync();

        var lotteries = await Context.Lotteries
            .Include(l => l.Prizes)
            .OrderByDescending(l => l.CreatedAt)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedList<Lottery>
        {
            Items = lotteries,
            TotalItems = totalItems,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task<Lottery> UpdateAsync(Lottery lottery)
    {
        lottery.UpdatedAt = DateTime.UtcNow;
        Context.Lotteries.Update(lottery);
        await Context.SaveChangesAsync();
        return lottery;
    }

    public async Task<Lottery?> GetLotteryAsync(Guid id)
        => await Context.Lotteries.FindAsync(id);

    public async Task<Lottery> DeleteLotteryAsync(Lottery lottery)
    {
        lottery.UpdatedAt = DateTime.UtcNow;
        lottery.DeletedAt = DateTime.UtcNow;
        Context.Lotteries.Update(lottery);
        await Context.SaveChangesAsync();
        return lottery;
    }
}