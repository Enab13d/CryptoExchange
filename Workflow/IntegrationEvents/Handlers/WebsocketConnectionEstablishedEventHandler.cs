using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;

namespace Workflow.IntegrationEvents.Handlers;

public class WebsocketConnectionEstablishedEventHandler(IWorkflowHost workflowHost, ILogger<WebsocketConnectionEstablishedEventHandler> logger) : IConsumer<WebsocketConnectionMessage>
{
    private readonly IWorkflowHost _workflowHost = workflowHost;

    private readonly ILogger<WebsocketConnectionEstablishedEventHandler> _logger = logger;
    public async Task Consume(ConsumeContext<WebsocketConnectionMessage> context)
    {
        WebsocketConnectionMessage message = context.Message;
        _logger.LogInformation("WebsocketConnectionEstablishedEvenyHandler: publishing event websocket-connection-established with payment id {id}", message.PaymentId);
        await _workflowHost.PublishEvent("websocket-connection-established", message.PaymentId.ToString(), message);

    }
}