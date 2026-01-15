using Newtonsoft.Json;

namespace UserService.Application.Services.DTO;

public record KcCredentials
{
    [JsonProperty("type")]
    public string Type { get; init; } = string.Empty;

    [JsonProperty("value")]
    public string Value { get; init; } = string.Empty;

    [JsonProperty("temporary")]
    public bool Temporary { get; init; }
}