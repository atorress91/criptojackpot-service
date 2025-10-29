using CryptoJackpotService.Ioc;
using CryptoJackpotService.Messaging.Configuration;
using CryptoJackpotService.Messaging.Events;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Worker.Extensions;
using CryptoJackpotService.Worker.Handlers;
using DotNetEnv;
using Microsoft.Extensions.Localization;

var builder = Host.CreateApplicationBuilder(args);

// Cargar variables de entorno según el ambiente
var envFile = builder.Environment.EnvironmentName.Equals("Development", StringComparison.OrdinalIgnoreCase)
    ? ".env.local"
    : ".env.prod";
Env.Load(envFile);

// Configurar archivo de configuración
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
