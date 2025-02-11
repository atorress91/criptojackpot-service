using CryptoJackpotService.Core.Middlewares;
using CryptoJackpotService.Ioc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.IocAppInjectDependencies();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

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