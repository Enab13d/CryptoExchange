using LiqPayProviderService.Domain.Constants;
using SharedContracts.Constants;

namespace LiqPayProviderService.Domain.Entities;

public class Payment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public Guid OrderId { get; init; }
    public Guid CorrelationId { get; set; }
    public Guid PaymentId { get; init; }
    public decimal Amount { get; set; }
    public Currency Fiat { get; set; }
    public Crypto Crypto { get; set; }
    public PaymentStatus Status { get; set; }
    public string WalletAddress { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; init; }

}