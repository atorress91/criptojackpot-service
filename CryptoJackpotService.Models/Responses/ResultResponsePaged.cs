using CryptoJackpotService.Models.Enums;

namespace CryptoJackpotService.Models.Responses;

public sealed record ResultResponsePaged<T>
{
    public bool Success { get; init; }
    public IEnumerable<T>? Data { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public string? Message { get; init; }
    public ErrorType? ErrorType { get; init; }

    public static ResultResponsePaged<T> Ok(IEnumerable<T> data, int pageNumber, int pageSize, int totalCount, int totalPages) =>
        new()
        {
            Success = true,
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

    public static ResultResponsePaged<T> Failure(ErrorType error, string message) =>
        new() { Success = false, ErrorType = error, Message = message };
}

