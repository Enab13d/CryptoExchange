using Newtonsoft.Json;

namespace UserService.Application.Services.DTO;


public record KcRefreshTokenResponseDTO
{
    [JsonProperty("access_token")]
    public string AcessToken { get; init; } = string.Empty;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonProperty("refresh_expires_in")]
    public int RefreshExpiresIn { get; init; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; init; } = string.Empty;

    [JsonProperty("token_type")]
    public string TokenType { get; init; } = string.Empty;
}