using System.Net;
using System.Text.Json;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Models.Exceptions;

public class BadRequestException : CustomException
{
    public BadRequestException(string message) : base(HttpStatusCode.BadRequest, CreateErrorResponse(message))
    {
    }

    private static string CreateErrorResponse(string message)
    {
        var response = new ServicesResponse
        {
            Success = false,
            Code = (int)HttpStatusCode.BadRequest,
            Message = message
        };
        return JsonSerializer.Serialize(response);
    }
}