using Amazon.S3;
using Amazon.S3.Model;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.Request;
using Microsoft.Extensions.Options;

namespace CryptoJackpotService.Core.Services;

public class DigitalOceanStorageService : IDigitalOceanStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly DigitalOceanSettings _settings;

    public DigitalOceanStorageService(IOptions<DigitalOceanSettings> settings)
    {
        _settings = settings.Value;

        var config = new AmazonS3Config
        {
            ServiceURL = settings.Value.Endpoint,
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(settings.Value.AccessKey, settings.Value.SecretKey, config);
    }

    public string GeneratePresignedUploadUrl(UploadRequest uploadRequest)
    {
        uploadRequest.ExpirationMinutes ??= 15;

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = uploadRequest.FileName,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(uploadRequest.ExpirationMinutes.Value),
            ContentType = uploadRequest.ContentType
        };

        return _s3Client.GetPreSignedURL(request);
    }
}