using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;

namespace Workflow.IntegrationEvents.Handlers;


public class LiqpayResponseReceivedEventHandler(IWorkflowHost workflowHost) : IConsumer<FiatToCryptoResponseMessage>
{
    private readonly IWorkflowHost _workflowHost = workflowHost;
    public async Task Consume(ConsumeContext<FiatToCryptoResponseMessage> context)
    {
        FiatToCryptoResponseMessage message = context.Message;
        await _workflowHost.PublishEvent("liqpay-response", message.CorrelationId.ToString(), message);
    }
}