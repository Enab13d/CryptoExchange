using Newtonsoft.Json;

namespace UserService.Application.Services.DTO;

public record KcLoginRequestDTO
//POST /realms/{realm}/protocol/openid-connect/token
{
    [JsonProperty("grant_type")]
    public string GrantType { get; init; } = string.Empty;

    [JsonProperty("client_id")]
    public string ClientId { get; init; } = string.Empty;

    [JsonProperty("client_secret")]
    public string ClientSecret { get; init; } = string.Empty;

    [JsonProperty("username")]
    public string Username { get; init; } = string.Empty;

    [JsonProperty("password")]
    public string Password { get; init; } = string.Empty;
}