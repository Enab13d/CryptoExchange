using Newtonsoft.Json;

namespace LiqPayProviderService.Domain.RequestParameters;


public class ILiqpayBasicApiParams
{
    [JsonProperty("version")]
    public int Version { get; set; }
    [JsonProperty("public_key")]
    public string PublicKey { get; set; } = string.Empty;


    [JsonProperty("sandbox")]
    public string Sandbox { get; set; } = string.Empty;
    [JsonIgnore]
    public bool IsSandbox
    {
        get
        {
            return Sandbox == "1";
        }
        set
        {
            Sandbox = value ? "1" : string.Empty;
        }
    }
}