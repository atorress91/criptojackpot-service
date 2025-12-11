using System.Net;

namespace CryptoJackpotService.Models.Exceptions;

public class ServiceUnavailableException : BaseException
{
    public ServiceUnavailableException(string message) 
        : base(message, HttpStatusCode.ServiceUnavailable)
    {
    }

    public ServiceUnavailableException(string message, Exception innerException) 
        : base(message, HttpStatusCode.ServiceUnavailable, innerException)
    {
    }
}

