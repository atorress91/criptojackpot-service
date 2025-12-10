using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Request.Pagination;
using CryptoJackpotService.Models.Request.Prize;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PrizeController(IPrizeService prizeService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePrize([FromBody] CreatePrizeRequest request)
    {
        var result = await prizeService.CreatePrizeAsync(request);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllPrizesAsync([FromQuery] PaginationRequest pagination)
    {
        var result = await prizeService.GetAllPrizesAsync(pagination);
        return result.ToActionResult();
    }
}