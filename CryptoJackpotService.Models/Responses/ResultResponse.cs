using System.Net;

namespace CryptoJackpotService.Models.Responses;

public record ResultResponse<T>
{
    public bool Success { get; }
    public T? Value { get; }
    public string? Error { get; }
    public HttpStatusCode StatusCode { get; }

    private ResultResponse(bool isSuccess, T? value, string? error,HttpStatusCode statusCode)
    {
        Success = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public static ResultResponse<T> Ok(T value) => new(true, value, null, HttpStatusCode.OK);
    public static ResultResponse<T> Failure(string error, HttpStatusCode statusCode) => new(false, default, error, statusCode);
}