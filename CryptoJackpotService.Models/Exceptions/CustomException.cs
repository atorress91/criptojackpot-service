using System.Net;
using System.Runtime.Serialization;

namespace CryptoJackpotService.Models.Exceptions;

public class CustomException : BaseException
{
    public CustomException() { }

    public CustomException(string message) : base(message) { }

    public CustomException(string message, Exception innerException) : base(message, innerException) { }

    public CustomException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public CustomException(HttpStatusCode statusCode, string exceptionBody)
    {
        StatusCode    = statusCode;
        ExceptionBody = exceptionBody;
    }

    public string? ExceptionBody { get; set; }
}