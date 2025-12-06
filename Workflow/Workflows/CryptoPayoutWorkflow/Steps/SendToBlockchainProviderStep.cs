using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Workflows.CryptoPayoutWorkflow.Steps;


public class SendToBlockchainProviderStep(IPublishEndpoint publishEndpoint, ILogger<SendToBlockchainProviderStep> logger) : StepBodyAsync
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly ILogger<SendToBlockchainProviderStep> _logger = logger;
    public CryptoPayoutMessage Payload { get; set; } = default!;
    public override async Task<WorkflowCore.Models.ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        await _publishEndpoint.Publish(Payload);
        _logger.LogInformation("Publishing payload from SendToBlockchainProviderStep: {address}, {crypto}", Payload.WalletAddress, Payload.Crypto);
        return WorkflowCore.Models.ExecutionResult.Next();

    }
}