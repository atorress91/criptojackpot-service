using System.Globalization;
using Microsoft.AspNetCore.Localization;
using CryptoJackpotService.Core.Middlewares;
using CryptoJackpotService.Ioc;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de localización
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configurar culturas soportadas
var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};

// Configurar para que use el header Accept-Language
localizationOptions.RequestCultureProviders.Clear();
localizationOptions.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = localizationOptions.DefaultRequestCulture;
    options.SupportedCultures = localizationOptions.SupportedCultures;
    options.SupportedUICultures = localizationOptions.SupportedUICultures;
    options.RequestCultureProviders = localizationOptions.RequestCultureProviders;
});

builder.Services.AddHealthChecks();
builder.Services.IocAppInjectDependencies();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

// Usar localización ANTES de otros middlewares
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
app.UseCors();

app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

app.UseMiddleware<ExceptionMiddleware>();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();