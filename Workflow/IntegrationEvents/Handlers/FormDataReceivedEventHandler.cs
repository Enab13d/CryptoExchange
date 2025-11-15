using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;

namespace Workflow.IntegrationEvents.Handlers;

public class FormDataReceivedEventHandler(IWorkflowHost workflowHost, ILogger<FormDataReceivedEventHandler> logger) : IConsumer<PaymentDataDTO>
{
    private readonly IWorkflowHost _workflowHost = workflowHost;
    private readonly ILogger<FormDataReceivedEventHandler> _logger = logger;
    public async Task Consume(ConsumeContext<PaymentDataDTO> context)
    {
        PaymentDataDTO paymentData = context.Message;
        _logger.LogInformation("FormDataReceivedEventHandler: publishing event form-data-prepared with correlation id {id}", paymentData.CorrelationId.ToString());
        await _workflowHost.PublishEvent("form-data-prepared", paymentData.CorrelationId.ToString(), paymentData);
    }
}