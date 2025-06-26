using System.Net;

namespace CryptoJackpotService.Models.Exceptions;

[Serializable]
public class BaseException : Exception
{
    public BaseException() { }

    public BaseException(string message) : base(message) { }

    public BaseException(string format, params object[] args) : base(string.Format(format, args)) { }

    public BaseException(string message, Exception innerException) : base(message, innerException) { }

    public BaseException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }
    
    public BaseException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public BaseException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public BaseException(string format, HttpStatusCode statusCode, params object[] args) : base(string.Format(format, args))
    {
        StatusCode = statusCode;
    }

    public BaseException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public BaseException(string format, HttpStatusCode statusCode, Exception innerException, params object[] args) : base(string.Format(format, args), innerException)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; set; }
}