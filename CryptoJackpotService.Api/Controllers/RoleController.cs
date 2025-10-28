using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RoleController(IRoleService roleService) : ControllerBase 
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await roleService.GetRolesAsync();
        return roles.ToActionResult();
    }
}


