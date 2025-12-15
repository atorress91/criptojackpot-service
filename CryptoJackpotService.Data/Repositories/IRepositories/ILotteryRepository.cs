using CryptoJackpotService.Data.Database.Custom;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Configuration;

namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface ILotteryRepository
{
    Task<Lottery> CreateAsync(Lottery lottery);
    Task<PagedList<Lottery>> GetAllLotteriesAsync(Pagination pagination);
}