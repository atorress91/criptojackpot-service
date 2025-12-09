using CryptoJackpotService.Ioc;
using CryptoJackpotService.Messaging.Configuration;
using CryptoJackpotService.Messaging.Events;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Worker.Extensions;
using CryptoJackpotService.Worker.Handlers;
using Microsoft.Extensions.Localization;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Agregar User Secrets en Development
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Configuration.AddEnvironmentVariables();

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

// Registrar los Consumers de Kafka
builder.Services.AddKafkaConsumer<UserCreatedEvent, UserCreatedEventHandler>();
builder.Services.AddKafkaConsumer<PasswordResetRequestedEvent, PasswordResetRequestedEventHandler>();

var host = builder.Build();
await host.RunAsync();
