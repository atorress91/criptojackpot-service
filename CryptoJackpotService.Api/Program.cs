using System.Globalization;
using Microsoft.AspNetCore.Localization;
using CryptoJackpotService.Core.Middlewares;
using CryptoJackpotService.Ioc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization();

// culturas soportadas
var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};

// Configurar para usar header Accept-Language
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
builder.Services.AddProblemDetails();  

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
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

app.UseMiddleware<ExceptionMiddleware>();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();