using Newtonsoft.Json;

namespace UserService.Application.Services.DTO;


public record KcLogoutRequestDTO
{
    [JsonProperty("client_id")]
    public required string ClientId { get; init; } = string.Empty;

    [JsonProperty("client_secret")]
    public required string ClientSecret { get; init; } = string.Empty;

    [JsonProperty("refresh_token")]
    public required string RefreshToken { get; init; } = string.Empty;
}