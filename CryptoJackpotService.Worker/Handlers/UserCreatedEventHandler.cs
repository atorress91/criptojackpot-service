using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Messaging.Consumers;
using CryptoJackpotService.Messaging.Events;
using CryptoJackpotService.Models.Request.Referral;
using CryptoJackpotService.Models.Resources;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Worker.Handlers;

public class UserCreatedEventHandler(
    ILogger<UserCreatedEventHandler> logger,
    IServiceScopeFactory serviceScopeFactory)
    : IEventHandler<UserCreatedEvent>
{
    public async Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Processing UserCreatedEvent: UserId={UserId}, Email={Email}, Name={Name} {LastName}",
            @event.UserId,
            @event.Email,
            @event.Name,
            @event.LastName);

        // Crear un scope para resolver servicios scoped
        using var scope = serviceScopeFactory.CreateScope();
        var brevoService = scope.ServiceProvider.GetRequiredService<IBrevoService>();
        var userReferralService = scope.ServiceProvider.GetRequiredService<IUserReferralService>();
        var localizer = scope.ServiceProvider.GetService<IStringLocalizer<ISharedResource>>();

        try
        {
            // 1. Enviar email de confirmación
            var emailSubject = localizer?["EmailConfirmationSubject"] ?? "Email Confirmation";

            var emailData = new Dictionary<string, string>
            {
                { "name", @event.Name },
                { "lastName", @event.LastName },
                { "token", @event.SecurityCode },
                { "user-email", @event.Email },
                { "subject", emailSubject }
            };

            logger.LogInformation("Sending confirmation email to {Email}", @event.Email);
            var emailResult = await brevoService.SendEmailConfirmationAsync(emailData);

            if (!emailResult.Success)
            {
                logger.LogWarning(
                    "Failed to send confirmation email for user {UserId}: {Error}",
                    @event.UserId,
                    emailResult.Message);

                // Re-lanzar la excepción para que Kafka reintente el procesamiento
                throw new Exception($"Failed to send confirmation email: {emailResult.Message}");
            }

            logger.LogInformation(
                "Confirmation email sent successfully to {Email} for user {UserId}",
                @event.Email,
                @event.UserId);

            // 2. Crear referral si aplica
            if (@event.ReferrerId.HasValue && !string.IsNullOrEmpty(@event.ReferralCode))
            {
                logger.LogInformation(
                    "Creating referral: User {UserId} referred by {ReferrerId} with code {ReferralCode}",
                    @event.UserId,
                    @event.ReferrerId.Value,
                    @event.ReferralCode);

                await userReferralService.CreateUserReferralAsync(new UserReferralRequest
                {
                    ReferredId = @event.UserId,
                    ReferrerId = @event.ReferrerId.Value,
                    ReferralCode = @event.ReferralCode
                });

                logger.LogInformation(
                    "Referral created successfully for user {UserId} by referrer {ReferrerId}",
                    @event.UserId,
                    @event.ReferrerId.Value);
            }
            else
            {
                logger.LogInformation("No referral code provided for user {UserId}", @event.UserId);
            }

            logger.LogInformation(
                "User creation workflow completed successfully for user {UserId}",
                @event.UserId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error processing UserCreatedEvent for user {UserId}. Error: {ErrorMessage}",
                @event.UserId,
                ex.Message);

            // Re-lanzar la excepción para que Kafka reintente el procesamiento
            throw;
        }
    }
}

