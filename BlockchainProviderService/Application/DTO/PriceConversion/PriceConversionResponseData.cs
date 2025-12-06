using Newtonsoft.Json;
namespace BlockchainProviderService.Application.DTO.PriceConversion;


public record PriceConversionResponseData
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("last_updated")]
    public DateTime LastUpdated { get; set; }

    [JsonProperty("quote")]
    public Dictionary<string, PriceConversionObject> Quote { get; set; } = [];

}