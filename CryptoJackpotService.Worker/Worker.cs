using Confluent.Kafka;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Messaging.Configuration;
using CryptoJackpotService.Messaging.Events;
using CryptoJackpotService.Models.Request.Referral;
using CryptoJackpotService.Models.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CryptoJackpotService.Worker;

public class UserCreatedConsumerWorker : BackgroundService
{
    private readonly ILogger<UserCreatedConsumerWorker> _logger;
    private readonly KafkaSettings _kafkaSettings;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IConsumer<string, string>? _consumer;

    public UserCreatedConsumerWorker(
        ILogger<UserCreatedConsumerWorker> logger,
        IOptions<KafkaSettings> kafkaSettings,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _kafkaSettings = kafkaSettings.Value;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("UserCreatedConsumerWorker starting...");

        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = _kafkaSettings.GroupId,
            EnableAutoCommit = _kafkaSettings.EnableAutoCommit,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            SessionTimeoutMs = _kafkaSettings.SessionTimeoutMs,
            EnableAutoOffsetStore = false
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(_kafkaSettings.UserEventsTopic);

        _logger.LogInformation("Subscribed to topic: {Topic}", _kafkaSettings.UserEventsTopic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    if (consumeResult?.Message?.Value == null)
                        continue;

                    _logger.LogInformation(
                        "Received message from topic {Topic}, partition {Partition}, offset {Offset}",
                        consumeResult.Topic,
                        consumeResult.Partition.Value,
                        consumeResult.Offset.Value);

                    var userCreatedEvent = JsonConvert.DeserializeObject<UserCreatedEvent>(consumeResult.Message.Value);

                    if (userCreatedEvent != null)
                    {
                        await ProcessUserCreatedEventAsync(userCreatedEvent, stoppingToken);
                        _consumer.StoreOffset(consumeResult);

                        _logger.LogInformation(
                            "Successfully processed UserCreatedEvent for user {UserId}",
                            userCreatedEvent.UserId);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming message from topic {Topic}: {Error}",
                        _kafkaSettings.UserEventsTopic, ex.Error.Reason);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from topic {Topic}",
                        _kafkaSettings.UserEventsTopic);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("UserCreatedConsumerWorker is shutting down");
        }
        finally
        {
            _consumer?.Close();
        }
    }

    private async Task ProcessUserCreatedEventAsync(UserCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Processing UserCreatedEvent: UserId={UserId}, Email={Email}, Name={Name} {LastName}",
            @event.UserId,
            @event.Email,
            @event.Name,
            @event.LastName);

        // Crear un scope para resolver servicios scoped
        using var scope = _serviceScopeFactory.CreateScope();
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

            _logger.LogInformation("Sending confirmation email to {Email}", @event.Email);
            var emailResult = await brevoService.SendEmailConfirmationAsync(emailData);
            
            if (!emailResult.Success)
            {
                _logger.LogWarning(
                    "Failed to send confirmation email for user {UserId}: {Error}",
                    @event.UserId,
                    emailResult.Message);
            }
            else
            {
                _logger.LogInformation(
                    "Confirmation email sent successfully to {Email} for user {UserId}",
                    @event.Email,
                    @event.UserId);
            }

            // 2. Crear referral si aplica
            if (@event.ReferrerId.HasValue && !string.IsNullOrEmpty(@event.ReferralCode))
            {
                _logger.LogInformation(
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

                _logger.LogInformation(
                    "Referral created successfully for user {UserId} by referrer {ReferrerId}",
                    @event.UserId,
                    @event.ReferrerId.Value);
            }
            else
            {
                _logger.LogInformation("No referral code provided for user {UserId}", @event.UserId);
            }

            _logger.LogInformation(
                "User creation workflow completed successfully for user {UserId}",
                @event.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error processing UserCreatedEvent for user {UserId}. Error: {ErrorMessage}",
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
