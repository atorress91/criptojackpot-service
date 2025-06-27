using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface ICountryService
{
    Task<ResultResponse<IEnumerable<CountryDto>>> GetCountriesAsync();
}