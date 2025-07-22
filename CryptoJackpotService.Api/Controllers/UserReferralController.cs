using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserReferralController : ControllerBase 
{
    private readonly IUserReferralService _userReferralService;

    public UserReferralController(IUserReferralService userReferralService)
    {
        _userReferralService = userReferralService;
    }

    [AllowAnonymous]
    [HttpGet("{userId:long}")]
    public async Task<IActionResult> GetUserReferralsAsync([FromRoute] long userId)
    {
        var result = await _userReferralService.GetReferralStatsAsync(userId);
        return result.ToActionResult();
    }
}