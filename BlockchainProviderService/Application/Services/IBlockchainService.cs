using SharedContracts.Constants;

namespace BlockchainProviderService.Application.Services;

//define strategy interface
public interface IBlockchainService
{
    public Task SendCryptoAsync(string walletAddress, Currency fiat, Crypto crypto, decimal amount);

}