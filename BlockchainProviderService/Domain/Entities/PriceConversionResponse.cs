using Newtonsoft.Json;
namespace BlockchainProviderService.Domain.Entities;


public record PriceConversionResponse
{
    [JsonProperty("data")]
    public PriceConversionResponseData Data { get; set; } = default!;
    [JsonProperty("status")]
    public CoinmarketcapResponseStatus Status { get; set; } = default!;

}