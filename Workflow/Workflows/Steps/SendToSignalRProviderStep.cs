using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Workflows.Steps;


public class SendToSignalRProviderStep : StepBodyAsync
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<SendToSignalRProviderStep> _logger;

    public SendToSignalRProviderStep(IPublishEndpoint publishEndpoint, ILogger<SendToSignalRProviderStep> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }
    public FiatToCryptoMessage Input
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
        _logger.LogInformation("Publishing payload to SendToSignalRProviderStep: {data}, {signature}", message.Data, message.Signature);
        await _publishEndpoint.Publish(message);
        return WorkflowCore.Models.ExecutionResult.Next();
    }
}