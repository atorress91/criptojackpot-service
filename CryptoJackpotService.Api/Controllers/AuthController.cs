using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(IAuthService authService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
    {
        var result = await authService.AuthenticateAsync(request);
        return result is null ? Fail("Invalid username or password") : Success(result);
    }
}