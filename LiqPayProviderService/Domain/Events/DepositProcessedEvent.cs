namespace LiqPayProviderService.Domain.Events
{
    public class DepositProcessedEvent
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}