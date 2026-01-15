using LiqPayProviderService.Commands;
using MassTransit;
using MediatR;
using SharedContracts;

namespace LiqPayProviderService.IntegrationEvents.Handlers;

public class DepositRequestedEventHandler(IMediator mediator) : IConsumer<FiatOnRampRequested>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<FiatOnRampRequested> context)
    {

        FiatOnRampRequested msg = context.Message;
        var command = new ProcessDepositCommand
        {
            CorrelationId = msg.CorrelationId,
            PaymentId = msg.PaymentId,
            Amount = msg.Amount,
            Fiat = msg.Fiat,
            Crypto = msg.Crypto,
            Description = msg.Description,
            OrderId = msg.OrderId,
            Phone = msg.Phone,
            WalletAddress = msg.WalletAddress,
            UserId = msg.UserId

        };

        await _mediator.Send(command);
    }
}
