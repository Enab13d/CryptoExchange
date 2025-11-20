using MassTransit;
using SharedContracts;
using Workflow.Services;

namespace Workflow.IntegrationEvents.Handlers;


public class LiqpayResponseReceivedEventHandler(IWorkflowService workflowService) : IConsumer<FiatToCryptoResponseMessage>
{
    private readonly IWorkflowService _workflowService = workflowService;
    public async Task Consume(ConsumeContext<FiatToCryptoResponseMessage> context)
    {
        FiatToCryptoResponseMessage msg = context.Message;
        CryptoPayoutMessage cryptoPayoutMessage = new()
        {
            CorrelationId = msg.CorrelationId,
            Crypto = msg.Crypto,
            Fiat = msg.Fiat,
            Amount = msg.Amount,
            WalletAddress = msg.WalletAddress
        };
        await _workflowService.StartCryptoPayoutWorkflow(cryptoPayoutMessage);
    }
}