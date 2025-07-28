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
        var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<ISharedResource>>();
        
        context.Response.ContentType = MediaTypeNames.Application.Json;

        return exception switch
        {
            CustomException customEx when !string.IsNullOrEmpty(customEx.ExceptionBody) => 
                HandleCustomExceptionWithBody(context, customEx),
            
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
}