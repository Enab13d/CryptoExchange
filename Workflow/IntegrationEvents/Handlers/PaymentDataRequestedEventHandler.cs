using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;

namespace Workflow.IntegrationEvents.Handlers;

public class PaymentDataRequestedEventHandler(IWorkflowHost workflowHost, ILogger<PaymentDataRequestedEventHandler> logger) : IConsumer<PaymentDataRequestedMessage>
{
    private readonly IWorkflowHost _workflowHost = workflowHost;

    private readonly ILogger<PaymentDataRequestedEventHandler> _logger = logger;
    public async Task Consume(ConsumeContext<PaymentDataRequestedMessage> context)
    {
        PaymentDataRequestedMessage message = context.Message;
        _logger.LogInformation("PaymentDataRequestedEventHandler: publishing event payment-data-requested with payment id {id}", message.PaymentId);
        await _workflowHost.PublishEvent("payment-data-requested", message.PaymentId.ToString(), message);

    }
}