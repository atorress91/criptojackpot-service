using CryptoJackpotService.Models.DTO;

namespace CryptoJackpotService.Core.Services.IServices;

public interface ICountryService
{
    Task<IEnumerable<CountryDto>> GetCountriesAsync();
}