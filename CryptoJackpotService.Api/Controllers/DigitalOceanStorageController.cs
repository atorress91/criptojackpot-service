using Asp.Versioning;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DigitalOceanStorageController: ControllerBase 
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
        var result = _digitalOceanStorageService.GeneratePresignedUploadUrl(uploadRequest);  

        return result.ToActionResult();  
    }
}