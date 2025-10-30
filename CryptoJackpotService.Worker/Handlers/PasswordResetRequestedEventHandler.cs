using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Messaging.Consumers;
using CryptoJackpotService.Messaging.Events;

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
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        try
        {
            // Enviar email de reset de contraseña
            logger.LogInformation("Sending password reset email to {Email}", @event.Email);
            
            await notificationService.SendPasswordResetEmailAsync(
                @event.Email,
                @event.Name,
                @event.LastName,
                @event.SecurityCode);


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

