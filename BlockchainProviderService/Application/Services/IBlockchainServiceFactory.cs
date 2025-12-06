using BlockchainProviderService.Application.Constants;

namespace BlockchainProviderService.Application.Services;

public interface IBlockchainServiceFactory
{
    IBlockchainService CreateService(Blockchain blockchain);
}

