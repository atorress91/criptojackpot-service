using Confluent.Kafka;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Messaging.Configuration;
using CryptoJackpotService.Messaging.Events;
using CryptoJackpotService.Models.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CryptoJackpotService.Worker;

public class PasswordResetConsumerWorker(
    ILogger<PasswordResetConsumerWorker> logger,
    IOptions<KafkaSettings> kafkaSettings,
    IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;
    private IConsumer<string, string>? _consumer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("PasswordResetConsumerWorker starting...");

        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = $"{_kafkaSettings.GroupId}-password-reset",
            EnableAutoCommit = _kafkaSettings.EnableAutoCommit,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            SessionTimeoutMs = _kafkaSettings.SessionTimeoutMs,
            EnableAutoOffsetStore = false
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(_kafkaSettings.UserEventsTopic);

        logger.LogInformation("Subscribed to topic: {Topic}", _kafkaSettings.UserEventsTopic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    if (consumeResult?.Message?.Value == null)
                        continue;

                    logger.LogInformation(
                        "Received message from topic {Topic}, partition {Partition}, offset {Offset}",
                        consumeResult.Topic,
                        consumeResult.Partition.Value,
                        consumeResult.Offset.Value);

                    // Intentar deserializar como PasswordResetRequestedEvent
                    try
                    {
                        var passwordResetEvent = JsonConvert.DeserializeObject<PasswordResetRequestedEvent>(consumeResult.Message.Value);

                        if (passwordResetEvent != null && !string.IsNullOrEmpty(passwordResetEvent.SecurityCode))
                        {
                            await ProcessPasswordResetEventAsync(passwordResetEvent, stoppingToken);
                            _consumer.StoreOffset(consumeResult);

                            logger.LogInformation(
                                "Successfully processed PasswordResetRequestedEvent for user {UserId}",
                                passwordResetEvent.UserId);
                        }
                    }
                    catch (JsonSerializationException)
                    {
                        // No es un evento de password reset, simplemente lo ignoramos
                        _consumer.StoreOffset(consumeResult);
                    }
                }
                catch (ConsumeException ex)
                {
                    logger.LogError(ex, "Error consuming message from topic {Topic}: {Error}",
                        _kafkaSettings.UserEventsTopic, ex.Error.Reason);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing message from topic {Topic}",
                        _kafkaSettings.UserEventsTopic);
                }
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("PasswordResetConsumerWorker is shutting down");
        }
        finally
        {
            _consumer?.Close();
        }
    }

    private async Task ProcessPasswordResetEventAsync(PasswordResetRequestedEvent @event, CancellationToken cancellationToken)
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
            else
            {
                logger.LogInformation(
                    "Password reset email sent successfully to {Email} for user {UserId}",
                    @event.Email,
                    @event.UserId);
            }
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

    public override void Dispose()
    {
        _consumer?.Dispose();
        base.Dispose();
    }
}

