using CryptoJackpotService.Data.Database.Custom;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Configuration;

namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface ILotteryRepository
{
    Task<Lottery> CreateAsync(Lottery lottery);
    Task<PagedList<Lottery>> GetAllLotteriesAsync(Pagination pagination);
    Task<Lottery?> GetLotteryAsync(Guid id);
    Task<Lottery> UpdateAsync(Lottery lottery);
    Task<Lottery> DeleteLotteryAsync(Lottery lottery);
}