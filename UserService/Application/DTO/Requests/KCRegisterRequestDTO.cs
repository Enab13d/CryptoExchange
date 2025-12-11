using Newtonsoft.Json;

namespace UserService.Application.DTO.Requests;

public class KCRegisterRequestDTO
{
    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonProperty("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonProperty("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("credentials")]
    public List<KCCredentials> Credentials { get; set; } = [];

    [JsonProperty("realmRoles")]
    public List<string> RealmRoles { get; set; } = [];
}