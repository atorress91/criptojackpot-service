namespace CryptoJackpotService.Models.Exceptions;

public abstract class BaseException : Exception
{
    public int StatusCode { get; }

    protected BaseException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}