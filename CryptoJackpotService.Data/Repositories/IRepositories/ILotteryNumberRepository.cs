using CryptoJackpotService.Data.Database.Models;

namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface ILotteryNumberRepository
{
    Task<IEnumerable<LotteryNumber>> GetNumbersByLotteryAsync(Guid lotteryId);
    Task<HashSet<int>> GetSoldNumbersAsync(Guid lotteryId);
    Task<bool> IsNumberAvailableAsync(Guid lotteryId, int number, int series);
    Task<List<int>> GetRandomAvailableNumbersAsync(Guid lotteryId, int count, int maxNumber);
}