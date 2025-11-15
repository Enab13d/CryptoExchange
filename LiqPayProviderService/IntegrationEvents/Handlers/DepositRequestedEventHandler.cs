using LiqPayProviderService.Commands;
using MassTransit;
using MediatR;
using SharedContracts;

namespace LiqPayProviderService.IntegrationEvents.Handlers;

public class DepositRequestedEventHandler(IMediator mediator) : IConsumer<FiatToCryptoMessage>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<FiatToCryptoMessage> context)
    {
        
        FiatToCryptoMessage msg = context.Message;
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

        };

        await _mediator.Send(command);
    }
}
