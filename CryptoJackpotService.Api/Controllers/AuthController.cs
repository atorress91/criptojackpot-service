using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : BaseController
{
    
    public AuthController()
    {
    }
    
    [HttpGet]
    public ActionResult<string> Get()
    {
        return "Hello World!";
    }
}