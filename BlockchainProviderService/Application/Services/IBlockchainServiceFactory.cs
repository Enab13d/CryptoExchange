using BlockchainProviderService.Domain.Entities;

namespace BlockchainProviderService.Application.Services;

public interface IBlockchainServiceFactory
{
    IBlockchainService CreateService(Blockchain blockchain);
}

