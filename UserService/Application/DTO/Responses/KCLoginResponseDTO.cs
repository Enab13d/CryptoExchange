using Newtonsoft.Json;

namespace UserService.Application.DTO.Responses;

public record KCLoginResponseDTO
{
    [JsonProperty("access_token")]
    public string AcessToken { get; set; } = string.Empty;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;

    [JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;

    [JsonProperty("not-before-policy")]
    public int NotBeforePolicy { get; set; }

    [JsonProperty("session_state")]
    public string SessionState { get; set; } = string.Empty;

    [JsonProperty("scope")]
    public string Scope { get; set; } = string.Empty;
}