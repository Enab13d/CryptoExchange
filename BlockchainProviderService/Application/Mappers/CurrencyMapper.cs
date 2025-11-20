using BlockchainProviderService.Infrastracture.Configuration;
using Microsoft.Extensions.Options;
using SharedContracts.Constants;

namespace BlockchainProviderService.Application.Mappers;


public class CurrencyMapper(IOptions<CryptoMarketDataClientOptions> options) : ICurrencyMapper
{
    private readonly IOptions<CryptoMarketDataClientOptions> _options = options;

    public int CryptoToConversionId(Crypto crypto) => _options.Value.CurrencyMappingOptions.CryptoIdMap[crypto];
    public int FiatToConverstionId(Currency fiat) => _options.Value.CurrencyMappingOptions.FiatIdMap[fiat];
}