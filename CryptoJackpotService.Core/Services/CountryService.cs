using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services;

public class CountryService(IMapper mapper, ICountryRepository countryRepository) : BaseService(mapper), ICountryService
{
    public async Task<ResultResponse<IEnumerable<CountryDto>>> GetCountriesAsync()
    {
        var countries = await countryRepository.GetAllCountries();
        var countriesDto = Mapper.Map<IEnumerable<CountryDto>>(countries);
        return ResultResponse<IEnumerable<CountryDto>>.Ok(countriesDto);
    }
}