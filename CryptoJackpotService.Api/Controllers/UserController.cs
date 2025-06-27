using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : BaseController
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
        return result.Success 
            ? Ok(result.Data)
            : StatusCode((int)result.Code, new { error = result.Message });
    }

    [Authorize]
    [HttpPatch("update-image-profile")]
    public async Task<IActionResult> UpdateImageProfile([FromBody] UpdateImageProfileRequest request)
    {
        var result = await _userService.UpdateImageProfile(request);
        return result.Success 
            ? Ok(result.Data)
            : StatusCode((int)result.Code, new { error = result.Message });
    }
}