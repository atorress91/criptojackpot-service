using System.Net;

namespace CryptoJackpotService.Models.Exceptions;

public class CustomException : BaseException
{
    public CustomException(string message, HttpStatusCode statusCode) 
        : base(message, statusCode) 
    {
    }

    public CustomException(string message, HttpStatusCode statusCode, Exception innerException) 
        : base(message, statusCode, innerException) 
    {
    }
    
    public CustomException(string message, HttpStatusCode statusCode, string exceptionBody)
        : base(message, statusCode)
    {
        ExceptionBody = exceptionBody;
    }

    public CustomException(string message, HttpStatusCode statusCode, string exceptionBody, Exception innerException)
        : base(message, statusCode, innerException)
    {
        ExceptionBody = exceptionBody;
    }

    public string? ExceptionBody { get; init; }
}