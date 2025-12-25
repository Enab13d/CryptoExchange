using SharedContracts;
using WorkflowCore.Interface;

namespace Workflow.Services
{
    public class WorkflowService(IWorkflowHost workflowHost) : IWorkflowService
    {
        private readonly IWorkflowHost _workflowHost = workflowHost;

        public async Task StartCryptoPayoutWorkflow(CryptoPayoutMessage payload)
        {
            await _workflowHost.StartWorkflow("CryptoPayoutWorkflow", 1, payload);
        }

        public async Task StartFiatOnRampWorkflowAsync(FiatOnRampRequested payload)
        {

            await _workflowHost.StartWorkflow("FiatOnRampWorkflow", 1, payload);
        }
    }
}
