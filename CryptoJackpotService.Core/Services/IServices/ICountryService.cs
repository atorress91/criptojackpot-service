using CryptoJackpotService.Models.DTO.Country;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface ICountryService
{
    Task<ResultResponse<IEnumerable<CountryDto>>> GetCountriesAsync();
}