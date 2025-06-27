using System.Net;

namespace CryptoJackpotService.Models.Exceptions;

[Serializable]
public abstract class BaseException : Exception
{
    protected BaseException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    protected BaseException(string message, HttpStatusCode statusCode, Exception innerException) 
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}