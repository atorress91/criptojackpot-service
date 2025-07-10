using System.Net;

namespace CryptoJackpotService.Models.Exceptions;

public abstract class BaseException : Exception
{
    public HttpStatusCode StatusCode { get; }

    protected BaseException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    protected BaseException(string message, HttpStatusCode statusCode, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}