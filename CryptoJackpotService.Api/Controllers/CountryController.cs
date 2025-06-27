using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CountryController(ICountryService countryService) : BaseController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetCountries()
    {
        var countries = await countryService.GetCountriesAsync();
        return countries.ToActionResult();
    }
}