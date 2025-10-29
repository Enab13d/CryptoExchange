namespace LiqPayProviderService.Services;


public interface IWebhookService
{
    public Task Publish(Guid correlationId, CancellationToken cancellationToken);
}