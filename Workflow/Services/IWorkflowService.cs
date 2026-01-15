using SharedContracts;

namespace Workflow.Services
{
    public interface IWorkflowService
    {
        Task StartFiatOnRampWorkflowAsync(FiatOnRampRequested payload);

        Task StartCryptoPayoutWorkflow(CryptoPayoutMessage payload);
    }
}
