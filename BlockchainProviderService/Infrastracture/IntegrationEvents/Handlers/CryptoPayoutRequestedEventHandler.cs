using BlockchainProviderService.Application.Commands;
using MediatR;
using MassTransit;
using SharedContracts;

namespace BlockchainProviderService.Infrastracture.IntegrationEvents.Handlers;

public class CryptoPayoutRequestedEventHandler(IMediator mediator, ILogger<CryptoPayoutRequestedEventHandler> logger) : IConsumer<CryptoPayoutMessage>
{
    private readonly IMediator _mediator = mediator;

    private readonly ILogger<CryptoPayoutRequestedEventHandler> _logger = logger;
    public async Task Consume(ConsumeContext<CryptoPayoutMessage> context)
    {
        CryptoPayoutMessage message = context.Message;

        ProcessCryptoPayoutCommand command = new()
        {
            CorrelationId = message.CorrelationId,
            Fiat = message.Fiat,
            Crypto = message.Crypto,
            Amount = message.Amount,
            WalletAddress = message.WalletAddress
        };
        await _mediator.Send(command);
        _logger.LogInformation("Send ProcessCryptoPayoutCommand with CorrelationId {id}", command.CorrelationId);
    }
}