using System.Reflection;
using System.Text;
using CryptoJackpotService.Core.Providers.IProviders;
using CryptoJackpotService.Models.Responses;
using Microsoft.Extensions.Logging;

namespace CryptoJackpotService.Core.Providers;

public class EmailTemplateProvider : IEmailTemplateProvider
{
    private readonly ILogger<EmailTemplateProvider> _logger;

    public EmailTemplateProvider(ILogger<EmailTemplateProvider> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<string>> GetTemplateAsync(string templateName)
    {
        try
        {
            var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var separator = Path.DirectorySeparatorChar;
            var pathFile = $"{workingDirectory}{separator}EmailTemplates{separator}{templateName}";
            var template = await File.ReadAllTextAsync(pathFile, Encoding.UTF8);

            return string.IsNullOrEmpty(template)
                ? Result<string>.Failure("Template is empty")
                : Result<string>.Success(template);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load template: {TemplateName}", templateName);
            return Result<string>.Failure($"Failed to load template: {ex.Message}");
        }
    }
}