using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DigitalOceanStorageController: BaseController
{
    private readonly IDigitalOceanStorageService _digitalOceanStorageService;

    public DigitalOceanStorageController(IDigitalOceanStorageService digitalOceanStorageService)
    {
        _digitalOceanStorageService = digitalOceanStorageService;
    }

    [Authorize]
    [HttpPost("presign")]
    public IActionResult GeneratePresignedUploadUrl([FromBody] UploadRequest uploadRequest)  
    {
        var url = _digitalOceanStorageService.GeneratePresignedUploadUrl(uploadRequest);  

        return Ok(new { url });  
    }
}