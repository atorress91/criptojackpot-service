using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Request.User;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateUserAsync(request);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPatch("update-image-profile")]
    public async Task<IActionResult> UpdateImageProfile([FromBody] UpdateImageProfileRequest request)
    {
        var result = await _userService.UpdateImageProfile(request);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPatch("generate-new-security-code")]
    public async Task<IActionResult> GenerateNewSecurityCode([FromBody] GenerateSecurityCodeRequest request)
    {
        var result = await _userService.GenerateNewSecurityCode(request.UserId);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPatch("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
    {
        var result = await _userService.UpdatePasswordAsync(request);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPut("{userId:long}")]
    public async Task<IActionResult> UpdateUserAsync(long userId, [FromBody] UpdateUserRequest request)
    {
        var result = await _userService.UpdateUserAsync(userId, request);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("{userId:long}")]
    public async Task<IActionResult> GetUserByIdAsync([FromRoute] long userId)
    {
        var result = await _userService.GetUserAsyncById(userId);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("get-all-users")]
    public async Task<IActionResult> GetAllUsersAsync([FromQuery] long excludeUserId)
    {
        var result = await _userService.GetAllUsersAsync(excludeUserId);
        return result.ToActionResult();
    }
}