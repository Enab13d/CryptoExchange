using MassTransit;
using SharedContracts;

namespace LiqPayProviderService.Services;


public class WebhookService(IPublishEndpoint publishEndpoint) : IWebhookService
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task Publish(Guid correlationId, CancellationToken cancellationToken)
    {
        // move publishing logic into webhook service
        // Publish event back to workflow

        await _publishEndpoint.Publish(new FiatToCryptoResponseMessage
        {
            CorrelationId = correlationId,
            Status = "DepositProcessed"

        }, cancellationToken);
        //await _workflowHost.PublishEvent("liqpay-response", message.CorrelationId.ToString(), message);

    }

}