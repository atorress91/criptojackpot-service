using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
namespace CryptoJackpotService.Data.Repositories;

public class LotteryNumberRepository(CryptoJackpotDbContext context) : BaseRepository(context), ILotteryNumberRepository
{
    public async Task<IEnumerable<LotteryNumber>> GetNumbersByLotteryAsync(Guid lotteryId)
        => await Context.LotteryNumbers.Where(x => x.LotteryId == lotteryId).ToListAsync();

    /// <summary>
    /// Obtiene solo los números vendidos (más eficiente para exclusión)
    /// </summary>
    public async Task<HashSet<int>> GetSoldNumbersAsync(Guid lotteryId)
        => (await Context.LotteryNumbers
            .Where(x => x.LotteryId == lotteryId && !x.IsAvailable)
            .Select(x => x.Number)
            .ToListAsync())
            .ToHashSet();

    /// <summary>
    /// Verifica si un número específico está disponible (O(1) en DB)
    /// </summary>
    public async Task<bool> IsNumberAvailableAsync(Guid lotteryId, int number, int series)
        => !await Context.LotteryNumbers
            .AnyAsync(x => x.LotteryId == lotteryId && x.Number == number && x.Series == series);

    /// <summary>
    /// Obtiene N números aleatorios disponibles directamente desde la DB
    /// </summary>
    public async Task<List<int>> GetRandomAvailableNumbersAsync(Guid lotteryId, int count, int maxNumber)
    {
        var soldNumbers = await Context.LotteryNumbers
            .Where(x => x.LotteryId == lotteryId)
            .Select(x => x.Number)
            .ToListAsync();

        var soldSet = soldNumbers.ToHashSet();
        
        // Generar números disponibles en memoria (más rápido que consultar todos)
        var availableNumbers = Enumerable.Range(1, maxNumber)
            .Where(n => !soldSet.Contains(n))
            .OrderBy(_ => Guid.NewGuid()) // Aleatorio
            .Take(count)
            .ToList();

        return availableNumbers;
    }
}