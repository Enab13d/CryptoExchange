using MassTransit;
using MediatR;
using SharedContracts;

public class ProcessDepositCommandHandler : IRequestHandler<ProcessDepositCommand, bool>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProcessDepositCommandHandler> _logger;

    public ProcessDepositCommandHandler(ILogger<ProcessDepositCommandHandler> logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(ProcessDepositCommand command, CancellationToken cancellationToken)
    {
        // Process deposit logic here
        _logger.LogInformation("Processing deposit for Command Id: {CommandId} with Amount: {Amount}", command.Id, command.Amount);
        // Publish event back to workflow
        /*
        await _publishEndpoint.Publish(new FiatToCryptoResponseMessage
        {
           CorrelationId = command.CorrelationId,
           Status = "DepositProcessed"
          
        }, cancellationToken);
        //await _workflowHost.PublishEvent("liqpay-response", message.CorrelationId.ToString(), message);

        */
        return true;
    }
}