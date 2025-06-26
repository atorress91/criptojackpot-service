using Microsoft.AspNetCore.Http;

namespace CryptoJackpotService.Models.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string message)
        : base(StatusCodes.Status400BadRequest, message) { }
}