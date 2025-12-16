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
    
    [HttpGet]
    public async Task<IActionResult> GetAllLotteriesAsync([FromQuery] PaginationRequest pagination)
    {
        var result = await lotteryService.GetAllLotteriesAsync(pagination);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("{lotteryId:guid}")]
    public async Task<IActionResult> GetLotteryByIdAsync([FromRoute] Guid lotteryId)
    {
        var result = await lotteryService.GetLotteryByIdAsync(lotteryId);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPut("{lotteryId:guid}")]
    public async Task<IActionResult> UpdateLotteryAsync([FromRoute] Guid lotteryId, [FromBody] UpdateLotteryRequest request)
    {
        var result = await lotteryService.UpdateLotteryAsync(lotteryId, request);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpDelete("{lotteryId:guid}")]
    public async Task<IActionResult> DeleteLotteryAsync([FromRoute] Guid lotteryId)
    {
        var result = await lotteryService.DeleteLotteryAsync(lotteryId);
        return result.ToActionResult();
    }
}

