using MassTransit;
using MediatR;
using SharedContracts;

public class DepositRequestedEventHandler : IConsumer<FiatToCryptoMessage>
{
    private readonly IMediator _mediator;

    public DepositRequestedEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<FiatToCryptoMessage> context)
    {
        var command = new ProcessDepositCommand 
        { 
            Id = context.Message.CorrelationId,
            Amount = 0m // You might want to set this to an appropriate value
        };

        await _mediator.Send(command);
    }
}