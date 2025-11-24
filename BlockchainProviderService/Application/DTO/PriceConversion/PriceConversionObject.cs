using Newtonsoft.Json;
namespace BlockchainProviderService.Application.DTO.PriceConversion;


public record PriceConversionObject
{

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("last_updated")]
    public DateTime LastUpdated { get; set; }
}