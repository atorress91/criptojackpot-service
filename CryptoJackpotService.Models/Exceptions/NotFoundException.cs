using Microsoft.AspNetCore.Http;

namespace CryptoJackpotService.Models.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message)
        : base(StatusCodes.Status404NotFound, message) { }
}