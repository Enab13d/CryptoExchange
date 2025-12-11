using Newtonsoft.Json;

namespace UserService.Application.DTO.Requests;

public record KCLoginRequestDTO
//POST /realms/{realm}/protocol/openid-connect/token
{
    [JsonProperty("grant_type")]
    public string GrantType { get; set; } = string.Empty;

    [JsonProperty("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [JsonProperty("client_secret")]
    public string ClientSecret { get; set; } = string.Empty;

    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;

    [JsonProperty("password")]
    public string Password { get; set; } = string.Empty;
}