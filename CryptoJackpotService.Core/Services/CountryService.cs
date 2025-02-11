using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.DTO;

namespace CryptoJackpotService.Core.Services;

public class CountryService(IMapper mapper, ICountryRepository countryRepository) : BaseService(mapper), ICountryService
{
    public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
    {
        var countries = await countryRepository.GetAllCountries();
        return Mapper.Map<IEnumerable<CountryDto>>(countries);
    }
}