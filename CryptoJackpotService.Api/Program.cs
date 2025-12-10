using CryptoJackpotService.Core.Middlewares;
using CryptoJackpotService.Ioc;

var builder = WebApplication.CreateBuilder(args);

// Configuración nativa de .NET
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// User Secrets se agrega automáticamente en Development por WebApplication.CreateBuilder
builder.Services.AddHealthChecks();
builder.Services.IocAppInjectDependencies(builder.Configuration, builder.Environment);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

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