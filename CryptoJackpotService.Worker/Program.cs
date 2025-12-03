using CryptoJackpotService.Ioc;
using CryptoJackpotService.Messaging.Configuration;
using CryptoJackpotService.Messaging.Events;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Worker.Extensions;
using CryptoJackpotService.Worker.Handlers;
using Microsoft.Extensions.Localization;

var builder = Host.CreateApplicationBuilder(args);

// Configuración nativa de .NET (orden de precedencia de menor a mayor):
// 1. appsettings.json
// 2. appsettings.{Environment}.json
// 3. User Secrets (solo en Development)
// 4. Variables de entorno (para producción via CI/CD, Docker, etc.)
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Inyectar todas las dependencias usando el método de extensión centralizado
builder.Services.IocWorkerInjectDependencies(builder.Configuration, builder.Environment.EnvironmentName);

// Configurar IStringLocalizer<ISharedResource> específico para Worker
builder.Services.AddSingleton<IStringLocalizer<ISharedResource>>(sp =>
{
    var factory = sp.GetRequiredService<IStringLocalizerFactory>();
    return new StringLocalizer<ISharedResource>(factory);
});

// Registrar el mapper de eventos a topics
builder.Services.AddSingleton<IEventTopicMapper, EventTopicMapper>();

// Registrar los Consumers de Kafka usando la nueva arquitectura escalable
// Para agregar un nuevo consumer, simplemente agrega una línea más:
builder.Services.AddKafkaConsumer<UserCreatedEvent, UserCreatedEventHandler>();
builder.Services.AddKafkaConsumer<PasswordResetRequestedEvent, PasswordResetRequestedEventHandler>();

var host = builder.Build();
await host.RunAsync();
