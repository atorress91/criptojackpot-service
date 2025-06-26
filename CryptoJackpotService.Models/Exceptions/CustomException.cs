using System.Net;

namespace CryptoJackpotService.Models.Exceptions;

public class CustomException : BaseException
{
    public CustomException() { }

    public CustomException(string message) : base(message) { }

    public CustomException(string message, Exception innerException) : base(message, innerException) { }
    
    public CustomException(HttpStatusCode statusCode, string exceptionBody)
    {
        StatusCode    = statusCode;
        ExceptionBody = exceptionBody;
    }

    public string? ExceptionBody { get; set; }
}