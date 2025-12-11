using System.Net;

namespace CryptoJackpotService.Models.Exceptions;

public class ConflictException : BaseException
{
    public ConflictException(string message) : base(message, HttpStatusCode.Conflict)
    {
    }

    public ConflictException(string message, Exception innerException) 
        : base(message, HttpStatusCode.Conflict, innerException)
    {
    }
}

