using SharedContracts;

namespace Workflow.Services
{
    public interface IWorkflowService
    {
        Task StartFiatOnRampWorkflowAsync(FiatOnRampMessage payload);

        Task StartCryptoPayoutWorkflow(CryptoPayoutMessage payload);
    }
}
