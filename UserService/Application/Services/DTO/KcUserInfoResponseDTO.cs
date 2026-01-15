using Newtonsoft.Json;

namespace UserService.Application.Services.DTO;

public record KcUserInfoResponseDTO
{
    [JsonProperty("sub")]
    public string Sub { get; init; } = string.Empty;

    [JsonProperty("email_verified")]
    public bool EmailVerified { get; init; }

    [JsonProperty("name")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("preferred_username")]
    public string PreferredUsername { get; init; } = string.Empty;

    [JsonProperty("given_name")]
    public string GivenName { get; init; } = string.Empty;

    [JsonProperty("family_name")]
    public string FamilyName { get; init; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; init; } = string.Empty;

}