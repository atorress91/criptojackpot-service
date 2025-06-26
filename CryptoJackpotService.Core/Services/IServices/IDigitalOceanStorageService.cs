using CryptoJackpotService.Models.Request;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IDigitalOceanStorageService
{
    string GeneratePresignedUploadUrl(UploadRequest uploadRequest);
}