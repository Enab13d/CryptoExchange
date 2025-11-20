using BlockchainProviderService.Application.Commands;

namespace BlockchainProviderService.Application.Services;

//define strategy interface
public interface IBlockchainService
{
    public Task PayoutCryptoAsync(ProcessCryptoPayoutCommand request);

}