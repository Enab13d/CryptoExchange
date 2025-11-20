using BlockchainProviderService.Domain.Entities;

namespace BlockchainProviderService.Application.Services;

public class BlockchainServiceFactory(IServiceProvider sp) : IBlockchainServiceFactory
{
    private readonly IServiceProvider _sp = sp;

    public IBlockchainService CreateService(Blockchain blockchain)
    {
        return blockchain switch
        {
            Blockchain.Ethereum => _sp.GetRequiredService<EthereumProviderService>(),
            _ => throw new ArgumentException($"Unsupported blockchain: {blockchain}", nameof(blockchain))
        };
    }
}