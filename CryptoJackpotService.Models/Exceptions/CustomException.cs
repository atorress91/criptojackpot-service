using Microsoft.AspNetCore.Http;

namespace CryptoJackpotService.Models.Exceptions;

public class InternalServerException : BaseException
{
    public InternalServerException(string message)
        : base(StatusCodes.Status500InternalServerError, message) { }
}