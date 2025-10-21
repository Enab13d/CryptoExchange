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

        public async Task StartFiatToCryptoWorkflowAsync(FiatToCryptoMessage payload)
        {
            payload.CorrelationId = Guid.NewGuid();
            await _workflowHost.StartWorkflow("FiatToCryptoWorkflow", 1, payload);
        }
    }
}
