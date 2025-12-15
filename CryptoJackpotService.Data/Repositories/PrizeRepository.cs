using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Custom;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Configuration;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Prize?> GetPrizeAsync(Guid id)
        => await Context.Prizes.FindAsync(id);

    public async Task<PagedList<Prize>> GetAllPrizesAsync(Pagination pagination)
    {
        var totalItems = await Context.Prizes.CountAsync();
        
        var prizes = await Context.Prizes
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedList<Prize>
        {
            Items = prizes,
            TotalItems = totalItems,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }
    
    public async Task<Prize> UpdatePrizeAsync(Prize prize)
    {
        prize.UpdatedAt = DateTime.UtcNow;
        Context.Prizes.Update(prize);
        await Context.SaveChangesAsync();
        return prize;
    }
}