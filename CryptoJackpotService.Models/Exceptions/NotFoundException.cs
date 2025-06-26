using System.Net;
using System.Text.Json;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Models.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message) : base(HttpStatusCode.NotFound, CreateErrorResponse(message))
    {
    }

    private static string CreateErrorResponse(string message)
    {
        var response = new ServicesResponse
        {
            Success = false,
            Code = (int)HttpStatusCode.NotFound,
            Message = message
        };
        return JsonSerializer.Serialize(response);
    }
}