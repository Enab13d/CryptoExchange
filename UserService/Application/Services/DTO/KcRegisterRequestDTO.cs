using Newtonsoft.Json;

namespace UserService.Application.Services.DTO;

public class KcRegisterRequestDTO
{
    [JsonProperty("username")]
    public string Username { get; init; } = string.Empty;
    [JsonProperty("email")]
    public string Email { get; init; } = string.Empty;

    [JsonProperty("firstName")]
    public string FirstName { get; init; } = string.Empty;

    [JsonProperty("lastName")]
    public string LastName { get; init; } = string.Empty;

    [JsonProperty("enabled")]
    public bool Enabled { get; init; }

    [JsonProperty("credentials")]
    public List<KcCredentials> Credentials { get; init; } = [];

    [JsonProperty("realmRoles")]
    public List<string> RealmRoles { get; init; } = [];
}