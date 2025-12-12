using CryptoJackpotService.Data.Database.Models;
namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface ILotteryRepository
{
    Task<Lottery> CreateAsync(Lottery lottery);
}