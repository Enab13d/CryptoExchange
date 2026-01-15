using SharedContracts.Constants;

namespace BlockchainProviderService.Application.Services;


public interface ICryptoMarketDataClient
{
    public Task<decimal> ConvertCryptoToFiatAsync(Crypto crypto, Currency fiat, decimal amount);

    public Task<decimal> ConvertFiatToCryptoAsync(Currency fiat, Crypto crypto, decimal amount);
}