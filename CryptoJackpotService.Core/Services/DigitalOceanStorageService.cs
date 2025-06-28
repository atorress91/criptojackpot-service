using Amazon.S3;
using Amazon.S3.Model;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Enums;
using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace CryptoJackpotService.Core.Services;

public class DigitalOceanStorageService : IDigitalOceanStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly ApplicationConfiguration _settings;
    private readonly IStringLocalizer<SharedResource> _localizer;
    public DigitalOceanStorageService(IOptions<ApplicationConfiguration> settings, IStringLocalizer<SharedResource> localizer)
    {
        _settings = settings.Value;
        _localizer = localizer;
        
        var config = new AmazonS3Config
        {
            ServiceURL = settings.Value.DigitalOceanSettings!.Endpoint,
            ForcePathStyle = true,
            AuthenticationRegion = settings.Value.DigitalOceanSettings.Region,
        };

        _s3Client = new AmazonS3Client(settings.Value.DigitalOceanSettings.AccessKey, settings.Value.DigitalOceanSettings.SecretKey, config);
    }

    public ResultResponse<string> GeneratePresignedUploadUrl(UploadRequest uploadRequest)
    {
        var extension = Path.GetExtension(uploadRequest.FileName).ToLower();
    
        if (!Constants.AllowedExtensions.Contains(extension))
            return ResultResponse<string>.Failure(ErrorType.BadRequest,_localizer[ValidationMessages.InvalidFileType]);
        
        var uniqueFileName = $"profile-photos/{Guid.NewGuid()}{extension}";
        uploadRequest.ExpirationMinutes ??= 15;

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.DigitalOceanSettings!.BucketName,
            Key = uniqueFileName,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(uploadRequest.ExpirationMinutes.Value),
            ContentType = uploadRequest.ContentType
        };
        
        var url = _s3Client.GetPreSignedURL(request);

        return ResultResponse<string>.Ok(url);
    }

    public string GetPresignedUrl(string key)
    {
        const int expiresIn = 60;
        
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.DigitalOceanSettings!.BucketName,
            Key = key,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(expiresIn)
        };
        
        return _s3Client.GetPreSignedURL(request);
    }
}