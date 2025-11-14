using SharedContracts.Constants;

namespace SharedContracts;


public record DepositDTO()
{
    public Guid CorrelationId { get; init; }

    public Guid PaymentId { get; init; }
    public required decimal Amount { get; init; }
    public required Currency Currency { get; init; }
    public required Crypto Crypto { get; init; }
    public required string Description { get; init; }
    public string OrderId { get; init; } = string.Empty;
    public required string Phone { get; init; }

}