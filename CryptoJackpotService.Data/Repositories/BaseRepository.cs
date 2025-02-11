using CryptoJackpotService.Data.Database;

namespace CryptoJackpotService.Data.Repositories;

public class BaseRepository
{
    protected readonly CryptoJackpotDbContext Context;

    protected BaseRepository(CryptoJackpotDbContext context)
        => Context = context;
}