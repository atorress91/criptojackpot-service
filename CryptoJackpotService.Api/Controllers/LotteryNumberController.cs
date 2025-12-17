using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Request.LotteryNumber;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LotteryNumberController(ILotteryNumberService lotteryNumberService) : ControllerBase
{
    [Authorize]
    [HttpGet("{lotteryId:guid}/available")]
    public async Task<IActionResult> GetAvailableNumbersAsync([FromRoute] Guid lotteryId, [FromQuery] int count = 10)
    {
        var result = await lotteryNumberService.GetAvailableNumbersAsync(lotteryId, count);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("{lotteryId:guid}/check")]
    public async Task<IActionResult> IsNumberAvailableAsync(
        [FromRoute] Guid lotteryId,
        [FromQuery] int number,
        [FromQuery] int series)
    {
        var result = await lotteryNumberService.IsNumberAvailableAsync(lotteryId, number, series);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("{lotteryId:guid}/reserve")]
    public async Task<IActionResult> ReserveNumbersAsync(
        [FromRoute] Guid lotteryId,
        [FromBody] ReserveNumbersRequest request)
    {
        var result = await lotteryNumberService.ReserveNumbersAsync(lotteryId, request.TicketId, request.Numbers, request.Series);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpDelete("release/{ticketId:guid}")]
    public async Task<IActionResult> ReleaseNumbersAsync([FromRoute] Guid ticketId)
    {
        var result = await lotteryNumberService.ReleaseNumbersAsync(ticketId);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("{lotteryId:guid}/stats")]
    public async Task<IActionResult> GetNumberStatsAsync([FromRoute] Guid lotteryId)
    {
        var result = await lotteryNumberService.GetNumberStatsAsync(lotteryId);
        return result.ToActionResult();
    }
}