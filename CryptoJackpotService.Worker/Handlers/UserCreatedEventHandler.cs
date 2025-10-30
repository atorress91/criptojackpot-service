using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Messaging.Consumers;
using CryptoJackpotService.Messaging.Events;
using CryptoJackpotService.Models.Request.Referral;

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
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        var userReferralService = scope.ServiceProvider.GetRequiredService<IUserReferralService>();

        try
        {
            // 1. Enviar email de confirmación
            logger.LogInformation("Sending confirmation email to {Email}", @event.Email);
            
            await notificationService.SendEmailConfirmationAsync(
                @event.UserId,
                @event.Email,
                @event.Name,
                @event.LastName,
                @event.SecurityCode);

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

                // 3. Notificar al referrer por email
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var referrerResult = await userService.GetUserAsyncById(@event.ReferrerId.Value);
                
                if (referrerResult.Success && referrerResult.Data != null)
                {
                    logger.LogInformation(
                        "Sending referral notification to referrer {ReferrerId}",
                        @event.ReferrerId.Value);

                    await notificationService.SendReferralNotificationAsync(
                        referrerResult.Data.Email,
                        referrerResult.Data.Name,
                        referrerResult.Data.LastName,
                        @event.Name,
                        @event.LastName,
                        @event.ReferralCode);

                    logger.LogInformation(
                        "Referral notification sent successfully to referrer {ReferrerId}",
                        @event.ReferrerId.Value);
                }
                else
                {
                    logger.LogWarning(
                        "Could not send referral notification: Referrer {ReferrerId} not found",
                        @event.ReferrerId.Value);
                }
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

