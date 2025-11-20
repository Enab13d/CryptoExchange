using Newtonsoft.Json;
namespace BlockchainProviderService.Domain.Entities;


public record PriceConversionObject
{

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("last_updated")]
    public DateTime LastUpdated { get; set; }
}