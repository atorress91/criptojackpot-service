using System.Net;
using System.Net.Mime;
using CryptoJackpotService.Models.Exceptions;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.AspNetCore.Http;

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
                exceptionBody = exception.Message;

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