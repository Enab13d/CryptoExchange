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
        var command = new ProcessDepositCommand
        {
            Id = context.Message.CorrelationId,
            CorrelationId = context.Message.CorrelationId,
            Amount = context.Message.Amount,
            Fiat = context.Message.Currency,
            Crypto = context.Message.Crypto,
            Description = context.Message.Description,
            OrderId = context.Message.OrderId,
            Phone = context.Message.Phone,
            Card = context.Message.Card,
            CardExpirationMonth = context.Message.CardExpirationMonth,
            CardExpirationYear = context.Message.CardExpirationYear,
            CardCVV = context.Message.CardCVV
        };

        await _mediator.Send(command);
    }
}
