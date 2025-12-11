using System.Net;
using System.Net.Mime;
using CryptoJackpotService.Models.Exceptions;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<ISharedResource>>();

        context.Response.ContentType = MediaTypeNames.Application.Json;

        return exception switch
        {
            CustomException customEx when !string.IsNullOrEmpty(customEx.ExceptionBody) =>
                HandleCustomExceptionWithBody(context, customEx),

            DbUpdateException dbEx when IsDuplicateKeyException(dbEx) =>
                HandleDuplicateKeyException(context, localizer),

            BaseException baseEx =>
                HandleBaseException(context, baseEx),

            _ => HandleGenericException(context, localizer)
        };
    }

    private static Task HandleCustomExceptionWithBody(HttpContext context, CustomException customException)
    {
        context.Response.StatusCode = (int)customException.StatusCode;
        return context.Response.WriteAsync(customException.ExceptionBody!);
    }

    private static Task HandleBaseException(HttpContext context, BaseException baseException)
    {
        context.Response.StatusCode = (int)baseException.StatusCode;

        var response = new ServicesResponse
        {
            Success = false,
            Code = context.Response.StatusCode,
            Message = baseException.Message
        };

        return context.Response.WriteAsync(response.ToJsonString());
    }

    private static Task HandleGenericException(HttpContext context, IStringLocalizer<ISharedResource> localizer)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ServicesResponse
        {
            Success = false,
            Code = context.Response.StatusCode,
            Message = localizer["Error"]
        };

        return context.Response.WriteAsync(response.ToJsonString());
    }

    private static Task HandleDuplicateKeyException(HttpContext context, IStringLocalizer<ISharedResource> localizer)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Conflict;

        var response = new ServicesResponse
        {
            Success = false,
            Code = context.Response.StatusCode,
            Message = localizer["DuplicateRecord"]
        };

        return context.Response.WriteAsync(response.ToJsonString());
    }

    /// <summary>
    /// Determina si la excepción es una violación de clave única/duplicada.
    /// Compatible con PostgreSQL, SQL Server, MySQL y SQLite.
    /// </summary>
    private static bool IsDuplicateKeyException(DbUpdateException ex)
    {
        var innerException = ex.InnerException;
        if (innerException == null) return false;

        var message = innerException.Message;
        var typeName = innerException.GetType().Name;

        // PostgreSQL: código 23505 (unique_violation)
        if (typeName.Contains("PostgresException") && message.Contains("23505"))
            return true;

        // SQL Server: número de error 2601 o 2627
        if (typeName.Contains("SqlException") && (message.Contains("2601") || message.Contains("2627")))
            return true;

        // MySQL: error 1062 (duplicate entry)
        if (typeName.Contains("MySqlException") && message.Contains("1062"))
            return true;

        // SQLite: UNIQUE constraint failed
        if (message.Contains("UNIQUE constraint failed", StringComparison.OrdinalIgnoreCase))
            return true;

        // Detección genérica por mensaje
        return message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("unique constraint", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("duplicate entry", StringComparison.OrdinalIgnoreCase);
    }
}