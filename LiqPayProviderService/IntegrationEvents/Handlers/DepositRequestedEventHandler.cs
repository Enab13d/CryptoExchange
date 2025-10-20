using MassTransit;
using MediatR;

public class DepositRequestedEventHandler : IConsumer<DepositRequestedEvent>
{
    private readonly IMediator _mediator;

    public DepositRequestedEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<DepositRequestedEvent> context)
    {
        var command = new ProcessDepositCommand 
        { 
            Id = context.Message.Id,
            Amount = context.Message.Amount
            // map other fields
        };

        await _mediator.Send(command);
    }
}