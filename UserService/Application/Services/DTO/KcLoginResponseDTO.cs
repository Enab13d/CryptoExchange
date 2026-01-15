using Newtonsoft.Json;

namespace UserService.Application.Services.DTO;

public record KcLoginResponseDTO
{
    [JsonProperty("access_token")]
    public string AcessToken { get; init; } = string.Empty;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; init; } = string.Empty;

    [JsonProperty("token_type")]
    public string TokenType { get; init; } = string.Empty;

    [JsonProperty("not-before-policy")]
    public int NotBeforePolicy { get; init; }

    [JsonProperty("session_state")]
    public string SessionState { get; init; } = string.Empty;

    [JsonProperty("scope")]
    public string Scope { get; init; } = string.Empty;
}