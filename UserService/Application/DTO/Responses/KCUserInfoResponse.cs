using Newtonsoft.Json;

namespace UserService.Application.DTO.Responses;

public record UserInfoResponseDTO
{
    [JsonProperty("sub")]
    public string Sub { get; set; } = string.Empty;

    [JsonProperty("email_verified")]
    public bool EmailVerified { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("preferred_username")]
    public string PreferredUsername { get; set; } = string.Empty;

    [JsonProperty("given_name")]
    public string GivenName { get; set; } = string.Empty;

    [JsonProperty("family_name")]
    public string FamilyName { get; set; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

}