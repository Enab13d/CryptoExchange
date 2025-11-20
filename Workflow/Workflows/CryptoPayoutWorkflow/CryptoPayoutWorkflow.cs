using SharedContracts;
using Workflow.Workflows.CryptoPayoutWorkflow.Steps;
using WorkflowCore.Interface;

namespace Workflow.Workflows.CryptoPayoutWorkflow;

public class CryptoPayoutWorkflow : IWorkflow<CryptoPayoutMessage>
{
    public string Id => "CryptoPayoutWorkflow";

    public int Version => 1;

    public void Build(IWorkflowBuilder<CryptoPayoutMessage> builder)
    {
        builder.StartWith<SendToBlockchainProviderStep>()
        .Input(step => step.Payload, data => data);
    }
}