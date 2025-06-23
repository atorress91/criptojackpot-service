using System.Net;

namespace CryptoJackpotService.Models.Responses;

public record ResultResponse<T>
{
    public bool Success { get; }
    public T? Data { get; }
    public string? Message { get; }
    public HttpStatusCode Code { get; }

    private ResultResponse(bool isSuccess, T? value, string? error,HttpStatusCode statusCode)
    {
        Success = isSuccess;
        Data = value;
        Message = error;
        Code = statusCode;
    }

    public static ResultResponse<T> Ok(T value) => new(true, value, null, HttpStatusCode.OK);
    public static ResultResponse<T> Failure(string error, HttpStatusCode statusCode) => new(false, default, error, statusCode);
}