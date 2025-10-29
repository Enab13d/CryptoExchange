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
            Guid correlationId = Guid.NewGuid();
            payload.CorrelationId = correlationId;
            payload.OrderId = correlationId;
            await _workflowHost.StartWorkflow("FiatToCryptoWorkflow", 1, payload);
        }
    }
}
