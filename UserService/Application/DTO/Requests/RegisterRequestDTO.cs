using Newtonsoft.Json;

namespace UserService.Application.DTO.Requests;


public record RegisterRequestDTO
{
    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;
    [JsonProperty("password")]
    public string Password { get; set; } = string.Empty;


    [JsonProperty("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonProperty("lastName")]
    public string LastName { get; set; } = string.Empty;
}