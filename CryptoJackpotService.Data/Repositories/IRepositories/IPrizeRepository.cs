using CryptoJackpotService.Data.Database.Models;
namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface IPrizeRepository
{
    Task<Prize> CreatePrizeAsync(Prize prize);
}