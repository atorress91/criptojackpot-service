using CryptoJackpotService.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CryptoJackpotService.Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        => (_next, _logger) = (next, logger);

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (BaseException bex)
        {
            _logger.LogWarning(bex, "Error de negocio: {Message}", bex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado");
            throw;
        }
    }
}