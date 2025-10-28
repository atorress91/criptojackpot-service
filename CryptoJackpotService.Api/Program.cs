using CryptoJackpotService.Core.Middlewares;
using CryptoJackpotService.Ioc;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

var envFile = builder.Environment.EnvironmentName.Equals("Development", StringComparison.OrdinalIgnoreCase)
    ? ".env.local"
    : ".env.prod";
Env.Load(envFile);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddHealthChecks();
builder.Services.IocAppInjectDependencies(builder.Configuration, builder.Environment);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoJackpotService API v1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);


app.MapHealthChecks("/health");
app.MapControllers();

await app.RunAsync();