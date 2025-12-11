using System.Net;
using System.Net.Mime;
using CryptoJackpotService.Core.Helpers;
using CryptoJackpotService.Models.Exceptions;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CryptoJackpotService.Core.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ILogger<ExceptionMiddleware> logger,
        IStringLocalizer<ISharedResource> localizer)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger, localizer);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        ILogger logger,
        IStringLocalizer<ISharedResource> localizer)
    {
        if (context.Response.HasStarted)
        {
            logger.LogWarning(
                "Response already started, cannot handle exception: {Type} - {Message}",
                exception.GetType().Name,
                exception.Message);
            return;
        }

        var (statusCode, message) = ClassifyException(exception, localizer, logger);

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = statusCode;

        if (statusCode == (int)HttpStatusCode.ServiceUnavailable)
            context.Response.Headers.RetryAfter = "5";

        var response = new ServicesResponse
        {
            Success = false,
            Code = statusCode,
            Message = message
        };

        await context.Response.WriteAsJsonAsync(response);
    }

    private static (int StatusCode, string Message) ClassifyException(
        Exception exception,
        IStringLocalizer<ISharedResource> localizer,
        ILogger? logger)
    {
        // 1. Excepciones de negocio con body custom
        if (exception is CustomException { ExceptionBody: not null } customEx)
            return ((int)customEx.StatusCode, customEx.ExceptionBody);

        // 2. Excepciones de negocio tipadas (BadRequest, NotFound, Conflict, etc.)
        if (exception is BaseException baseEx)
            return ((int)baseEx.StatusCode, baseEx.Message);

        // 3. Violación de constraint único (DB)
        if (exception is DbUpdateException && DatabaseExceptionClassifier.IsDuplicateKeyException(exception))
            return ((int)HttpStatusCode.Conflict, localizer["DuplicateRecord"]);

        // 4. Errores transitorios de BD (pool agotado, conexión rechazada)
        if (DatabaseExceptionClassifier.IsTransientException(exception))
            return ((int)HttpStatusCode.ServiceUnavailable, localizer["ServiceTemporarilyUnavailable"]);

        // 5. Error genérico - loggear para investigar
        logger?.LogError(exception,
            "Unhandled exception: {Type} - {Message}",
            exception.GetType().Name,
            exception.Message);

        return ((int)HttpStatusCode.InternalServerError, localizer["Error"]);
    }
}