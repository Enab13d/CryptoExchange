namespace BlockchainProviderService.Infrastracture.Configuration;

public class CryptoMarketDataClientOptions
{
    public string ProviderAPIURL { get; set; } = string.Empty;

    public string PrivateKey { get; set; } = string.Empty;

    public CurrencyMappingOptions CurrencyMappingOptions { get; set; } = default!;

}