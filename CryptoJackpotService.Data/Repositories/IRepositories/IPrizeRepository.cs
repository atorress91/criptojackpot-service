using CryptoJackpotService.Data.Database.Custom;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Configuration;

namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface IPrizeRepository
{
    Task<Prize> CreatePrizeAsync(Prize prize);
    Task<Prize?> GetPrizeAsync(Guid id);
    Task<PagedList<Prize>> GetAllPrizesAsync(Pagination pagination);
    Task<Prize> UpdatePrizeAsync(Prize prize);
    Task<Prize> DeletePrizeAsync(Prize prize);
}