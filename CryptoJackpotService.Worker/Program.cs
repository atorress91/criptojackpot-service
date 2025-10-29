using CryptoJackpotService.Ioc;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Worker;
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

// Registrar los Workers (Consumers de Kafka)
builder.Services.AddHostedService<UserCreatedConsumerWorker>();
builder.Services.AddHostedService<PasswordResetConsumerWorker>();

var host = builder.Build();
await host.RunAsync();
