using LiqPayProviderService.Domain.Events;
using MassTransit;
using MediatR;

public class ProcessDepositCommandHandler : IRequestHandler<ProcessDepositCommand, bool>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProcessDepositCommandHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(ProcessDepositCommand command, CancellationToken cancellationToken)
    {
        // Process deposit logic here

        // Publish event back to workflow
        await _publishEndpoint.Publish(new DepositProcessedEvent
        {
            Id = command.Id,
            Status = "Completed"
        }, cancellationToken);

        return true;
    }
}