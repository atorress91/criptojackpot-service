using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Messaging.Consumers;
using CryptoJackpotService.Messaging.Events;
using CryptoJackpotService.Models.Resources;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Worker.Handlers;

public class PasswordResetRequestedEventHandler(
    ILogger<PasswordResetRequestedEventHandler> logger,
    IServiceScopeFactory serviceScopeFactory)
    : IEventHandler<PasswordResetRequestedEvent>
{
    public async Task HandleAsync(PasswordResetRequestedEvent @event, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Processing PasswordResetRequestedEvent: UserId={UserId}, Email={Email}, Name={Name} {LastName}",
            @event.UserId,
            @event.Email,
            @event.Name,
            @event.LastName);

        // Crear un scope para resolver servicios scoped
        using var scope = serviceScopeFactory.CreateScope();
        var brevoService = scope.ServiceProvider.GetRequiredService<IBrevoService>();
        var localizer = scope.ServiceProvider.GetService<IStringLocalizer<ISharedResource>>();

        try
        {
            // Enviar email de reset de contraseña
            var emailSubject = localizer?["PasswordResetSubject"] ?? "Password Reset Request";

            var emailData = new Dictionary<string, string>
            {
                { "name", @event.Name },
                { "lastName", @event.LastName },
                { "securityCode", @event.SecurityCode },
                { "user-email", @event.Email },
                { "subject", emailSubject }
            };

            logger.LogInformation("Sending password reset email to {Email}", @event.Email);
            var emailResult = await brevoService.SendPasswordResetEmailAsync(emailData);

            if (!emailResult.Success)
            {
                logger.LogWarning(
                    "Failed to send password reset email for user {UserId}: {Error}",
                    @event.UserId,
                    emailResult.Message);

                // Re-lanzar la excepción para que Kafka reintente el procesamiento
                throw new Exception($"Failed to send password reset email: {emailResult.Message}");
            }

            logger.LogInformation(
                "Password reset email sent successfully to {Email} for user {UserId}",
                @event.Email,
                @event.UserId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error processing PasswordResetRequestedEvent for user {UserId}. Error: {ErrorMessage}",
                @event.UserId,
                ex.Message);

            // Re-lanzar la excepción para que Kafka reintente el procesamiento
            throw;
        }
    }
}

