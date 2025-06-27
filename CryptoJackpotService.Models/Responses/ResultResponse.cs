using CryptoJackpotService.Models.Enums;

namespace CryptoJackpotService.Models.Responses;

public sealed record ResultResponse<T>
{
    public bool Success    { get; init; }
    public T?   Data       { get; init; }
    public string? Message { get; init; }
    public ErrorType? ErrorType { get; init; }

    public static ResultResponse<T> Ok(T data) =>
        new() { Success = true, Data = data };

    public static ResultResponse<T> Failure(ErrorType error, string message) =>
        new() { Success = false, ErrorType = error, Message = message };
}