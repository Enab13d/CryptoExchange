using LiqPayProviderService.Commands;
using MassTransit;
using MediatR;
using SharedContracts;

namespace LiqPayProviderService.IntegrationEvents.Handlers;

public class DepositRequestedEventHandler(IMediator mediator) : IConsumer<FiatOnRampMessage>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<FiatOnRampMessage> context)
    {

        FiatOnRampMessage msg = context.Message;
        var command = new ProcessDepositCommand
        {
            Id = msg.CorrelationId,
            CorrelationId = msg.CorrelationId,
            PaymentId = msg.PaymentId,
            Amount = msg.Amount,
            Fiat = msg.Currency,
            Crypto = msg.Crypto,
            Description = msg.Description,
            OrderId = msg.OrderId,
            Phone = msg.Phone,
            WalletAddress = msg.WalletAddress,
            User = msg.User

        };

        await _mediator.Send(command);
    }
}
