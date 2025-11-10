using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Workflows.Steps;


public class SendToSignalRProviderStep(IPublishEndpoint publishEndpoint, ILogger<SendToSignalRProviderStep> logger) : StepBodyAsync
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly ILogger<SendToSignalRProviderStep> _logger = logger;

    public PaymentDataDTO Payload
    { get; set; } = default!;

    public override async Task<WorkflowCore.Models.ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        _logger.LogInformation("Publishing payload from SendToSignalRProviderStep: {data}, {signature}", Payload.Data, Payload.Signature);
        await _publishEndpoint.Publish(Payload);
        return WorkflowCore.Models.ExecutionResult.Next();
    }
}