using SharedContracts.Constants;

namespace BlockchainProviderService.Infrastracture.Configuration;

public class CurrencyMappingOptions
{
    public Dictionary<Crypto, int> CryptoIdMap { get; set; } = default!;
    public Dictionary<Currency, int> FiatIdMap { get; set; } = default!;
}