using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Request.Lottery;
using CryptoJackpotService.Models.Request.Pagination;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LotteryController(ILotteryService lotteryService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateLottery([FromBody] CreateLotteryRequest request)
    {
        var result = await lotteryService.CreateLotteryAsync(request);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllLotteriesAsync([FromQuery] PaginationRequest pagination)
    {
        var result = await lotteryService.GetAllLotteriesAsync(pagination);
        return result.ToActionResult();
    }
}

