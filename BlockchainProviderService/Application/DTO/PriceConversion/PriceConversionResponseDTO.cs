using Newtonsoft.Json;
namespace BlockchainProviderService.Application.DTO.PriceConversion;


public record PriceConversionResponseDTO
{
    [JsonProperty("data")]
    public PriceConversionResponseData Data { get; set; } = default!;
    [JsonProperty("status")]
    public CoinmarketcapResponseStatus Status { get; set; } = default!;

}