using System.Net;
using System.Net.Mime;
using CryptoJackpotService.Models.Exceptions;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
        string exceptionBody;

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case CustomException customException:
                context.Response.StatusCode = (int)customException.StatusCode;
                exceptionBody = customException.ExceptionBody ?? string.Empty;
                return context.Response.WriteAsync(exceptionBody);

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                exceptionBody = localizer["Error"];

                var response = new ServicesResponse
                {
                    Success = false,
                    Code = context.Response.StatusCode,
                    Message = exceptionBody
                };

                return context.Response.WriteAsync(response.ToJsonString());
        }
    }
}