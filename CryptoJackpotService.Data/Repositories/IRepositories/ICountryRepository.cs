using CryptoJackpotService.Data.Database.Models;

namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface ICountryRepository
{
    Task<List<Country>> GetAllCountries();
}