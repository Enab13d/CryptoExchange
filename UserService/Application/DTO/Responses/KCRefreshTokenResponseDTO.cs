using Newtonsoft.Json;

namespace UserService.Application.DTO.Responses;


public record KCRefreshTokenResponseDTO
{
    [JsonProperty("access_token")]
    public string AcessToken { get; set; } = string.Empty;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("refresh_expires_in")]
    public int RefreshExpiresIn { get; set; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;

    [JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;
}