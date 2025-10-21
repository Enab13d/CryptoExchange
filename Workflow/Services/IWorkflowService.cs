using SharedContracts;

namespace Workflow.Services
{
    public interface IWorkflowService
    {
        Task StartFiatToCryptoWorkflowAsync(FiatToCryptoMessage payload);
    }
}
