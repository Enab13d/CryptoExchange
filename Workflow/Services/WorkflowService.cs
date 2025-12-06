using SharedContracts;
using WorkflowCore.Interface;

namespace Workflow.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowHost _workflowHost;

        public WorkflowService(IWorkflowHost workflowHost)
        {
            _workflowHost = workflowHost;
        }

        public async Task StartCryptoPayoutWorkflow(CryptoPayoutMessage payload)
        {
            await _workflowHost.StartWorkflow("CryptoPayoutWorkflow", 1, payload);
        }

        public async Task StartFiatOnRampWorkflowAsync(FiatOnRampMessage payload)
        {

            await _workflowHost.StartWorkflow("FiatOnRampWorkflow", 1, payload);
        }
    }
}
