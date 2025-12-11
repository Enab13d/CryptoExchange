using Newtonsoft.Json;

namespace UserService.Application.DTO.Requests;


public record ResetPasswordRequestDTO
{
    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;
}