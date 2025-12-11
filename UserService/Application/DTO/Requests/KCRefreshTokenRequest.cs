using Newtonsoft.Json;

namespace UserService.Application.DTO.Requests;


public record KCRefreshTokenRequest
{
    [JsonProperty("grant_type")]
    public required string GrantType { get; set; } = string.Empty;

    [JsonProperty("client_id")]
    public required string ClientId { get; set; } = string.Empty;

    [JsonProperty("client_secret")]
    public required string ClientSecret { get; set; } = string.Empty;

    [JsonProperty("refresh_token")]
    public required string RefreshToken { get; set; } = string.Empty;
}