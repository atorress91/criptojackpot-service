using System.Net;
using CryptoJackpotService.Models.Exceptions;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Utility.Extensions;

namespace CryptoJackpotService.Utility.Helpers;

public static class ExceptionFactory
{
    public static CustomException BadRequest(string message)
    {
        var errorResponse = new ServicesResponse
        {
            Success = false,
            Code = (int)HttpStatusCode.BadRequest,
            Message = message
        };

        return new CustomException(HttpStatusCode.BadRequest, errorResponse.ToJsonString());
    }

    public static CustomException NotFound(string message)
    {
        var errorResponse = new ServicesResponse
        {
            Success = false,
            Code = (int)HttpStatusCode.NotFound,
            Message = message
        };

        return new CustomException(HttpStatusCode.NotFound, errorResponse.ToJsonString());
    }

    public static CustomException Unauthorized(string message = "No autorizado")
    {
        var errorResponse = new ServicesResponse
        {
            Success = false,
            Code = (int)HttpStatusCode.Unauthorized,
            Message = message
        };

        return new CustomException(HttpStatusCode.Unauthorized, errorResponse.ToJsonString());
    }

    public static CustomException Forbidden(string message = "Acceso denegado")
    {
        var errorResponse = new ServicesResponse
        {
            Success = false,
            Code = (int)HttpStatusCode.Forbidden,
            Message = message
        };

        return new CustomException(HttpStatusCode.Forbidden, errorResponse.ToJsonString());
    }

    public static CustomException Conflict(string message)
    {
        var errorResponse = new ServicesResponse
        {
            Success = false,
            Code = (int)HttpStatusCode.Conflict,
            Message = message
        };

        return new CustomException(HttpStatusCode.Conflict, errorResponse.ToJsonString());
    }
}


