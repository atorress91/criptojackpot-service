using CryptoJackpotService.Models.Request.DigitalOcean;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IDigitalOceanStorageService
{
    ResultResponse<string> GeneratePresignedUploadUrl(UploadRequest uploadRequest);
    string GetPresignedUrl(string key);
}