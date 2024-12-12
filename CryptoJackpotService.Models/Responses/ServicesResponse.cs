using System.Text.Json.Serialization;

namespace CryptoJackpotService.Models.Responses;

public class ServicesResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("data")] public object? Data { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;

    [JsonPropertyName("code")] public int Code { get; set; }
}