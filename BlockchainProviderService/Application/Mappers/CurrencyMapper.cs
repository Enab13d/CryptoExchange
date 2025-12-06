using BlockchainProviderService.Application.Constants;
using BlockchainProviderService.Infrastracture.Configuration;
using Microsoft.Extensions.Options;
using SharedContracts.Constants;

namespace BlockchainProviderService.Application.Mappers;


public class CurrencyMapper(IOptions<CryptoMarketDataClientOptions> options) : ICurrencyMapper
{
    private readonly IOptions<CryptoMarketDataClientOptions> _options = options;

    public int CryptoToConversionId(Crypto crypto)
    {
        if (Enum.TryParse<CryptoId>(crypto.ToString(), out var id)) return (int)id;
        throw new ArgumentException("Unable to parse CryptoId. Unsupported crypto", nameof(crypto));
    }
    public int FiatToConverstionId(Currency fiat)
    {
        if (Enum.TryParse<FiatId>(fiat.ToString(), out var id)) return (int)id;
        throw new ArgumentException("Unable to parse FiatId. Unsupported fiat", nameof(fiat));
    }
}