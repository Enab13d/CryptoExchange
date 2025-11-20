using BlockchainProviderService.Application.Services;
using MassTransit;
using MediatR;
using SharedContracts;

namespace BlockchainProviderService.Application.Commands.Handlers;


public class ProcessCryptoPayoutCommandHandler(IPublishEndpoint publishEndpoint, IBlockchainServiceFactory blockchainServiceFactory) : IRequestHandler<ProcessCryptoPayoutCommand, bool>
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    private readonly IBlockchainServiceFactory _blockchainServiceFactory = blockchainServiceFactory;
    public async Task<bool> Handle(ProcessCryptoPayoutCommand request, CancellationToken cancellationToken)
    {
        IBlockchainService svc = _blockchainServiceFactory.CreateService(Domain.Entities.Blockchain.Ethereum);
        await svc.PayoutCryptoAsync(request);
        // await _publishEndpoint.Publish("");
        return true;

    }
}