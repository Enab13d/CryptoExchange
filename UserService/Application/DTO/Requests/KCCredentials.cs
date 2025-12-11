using Newtonsoft.Json;

namespace UserService.Application.DTO.Requests;

public record KCCredentials
{
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("value")]
    public string Value { get; set; } = string.Empty;

    [JsonProperty("temporary")]
    public bool Temporary { get; set; }
}