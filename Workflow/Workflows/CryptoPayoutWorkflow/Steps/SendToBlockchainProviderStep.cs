using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Workflows.CryptoPayoutWorkflow.Steps;


public class SendToBlockchainProviderStep(ISendEndpointProvider sendEndpointProvider, ILogger<SendToBlockchainProviderStep> logger) : StepBodyAsync
{
    private readonly ISendEndpointProvider _sendEndpointProvider = sendEndpointProvider;
    private readonly ILogger<SendToBlockchainProviderStep> _logger = logger;
    public CryptoPayoutMessage Payload { get; set; } = default!;
    public override async Task<WorkflowCore.Models.ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:crypto-payout-requested"));
        await endpoint.Send(Payload);
        _logger.LogInformation("Sending payload from SendToBlockchainProviderStep: {address}, {crypto}", Payload.WalletAddress, Payload.Crypto);
        return WorkflowCore.Models.ExecutionResult.Next();

    }
}