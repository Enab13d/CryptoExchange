using Newtonsoft.Json;
namespace BlockchainProviderService.Application.DTO.PriceConversion;


public record CoinmarketcapResponseStatus
{

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonProperty("error_code")]
    public int ErrorCode { get; set; }
    [JsonProperty("error_message")]
    public string ErrorMessage { get; set; } = string.Empty;

    [JsonProperty("elapsed")]
    public int Elapsed { get; set; }
    [JsonProperty("credit_count")]
    public int CreditCount { get; set; }
    [JsonProperty("notice")]
    public string Notice { get; set; } = string.Empty;

}