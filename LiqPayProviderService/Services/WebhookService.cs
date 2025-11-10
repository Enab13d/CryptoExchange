using LiqPayProviderService.Domain.Constants;
using MassTransit;
using SharedContracts;

namespace LiqPayProviderService.Services;


public class WebhookService(IPublishEndpoint publishEndpoint) : IWebhookService
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task Publish(Guid correlationId, DepositStatus status, CancellationToken cancellationToken)
    {
        // move publishing logic into webhook service
        // Publish event back to workflow
        DateTime timestamp = DateTime.Now;
        await _publishEndpoint.Publish(new FiatToCryptoResponseMessage
        {
            CorrelationId = correlationId,
            Status = status,
            CreateDate = timestamp,
            UpdateDate = timestamp

        }, cancellationToken);

    }

}