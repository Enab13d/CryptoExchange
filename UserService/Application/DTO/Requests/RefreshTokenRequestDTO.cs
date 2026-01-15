using Newtonsoft.Json;

namespace UserService.Application.DTO.Requests;


public record RefreshTokenRequestDTO
{
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
}