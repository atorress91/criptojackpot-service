using System.Net;

namespace CryptoJackpotService.Models.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message)
        : base(message, HttpStatusCode.NotFound) { }
}