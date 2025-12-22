using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Workflows.FiatOnRampWorkflow.Steps;


public class SendToSignalRProviderStep : StepBodyAsync
{


    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly ILogger<SendToSignalRProviderStep> _logger;

    public SendToSignalRProviderStep(ISendEndpointProvider sendEndpointProvider, ILogger<SendToSignalRProviderStep> logger)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _logger = logger;
    }
    public FiatOnRampMessage Input
    { get; set; } = default!;

    public override async Task<WorkflowCore.Models.ExecutionResult> RunAsync(IStepExecutionContext context)
    {

        PreparedFormDataMessage message = new()
        {
            CorrelationId = Input.CorrelationId,
            PaymentId = Input.PaymentId,
            Data = Input.PaymentData.Data,
            Signature = Input.PaymentData.Signature
        };
        _logger.LogInformation("Sending payload from SendToSignalRProviderStep: {data}, {signature}", message.Data, message.Signature);
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:payment-data-prepared"));
        await endpoint.Send(message);
        return WorkflowCore.Models.ExecutionResult.Next();
    }
}