using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CryptoJackpotService.Data.Repositories;

public class CountryRepository(CryptoJackpotDbContext context) : BaseRepository(context), ICountryRepository
{
    public Task<List<Country>> GetAllCountries()
        => Context.Countries.AsNoTracking().ToListAsync();
}