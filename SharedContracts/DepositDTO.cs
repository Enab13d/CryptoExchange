namespace SharedContracts;


public record DepositDTO()
{
    public Guid CorrelationId { get; init; }
    public required decimal Amount { get; init; }
    public required string Currency { get; init; }
    public required string Description { get; init; }
    public string OrderId { get; init; } = string.Empty;
    public required string Phone { get; init; }
    // public required string Card { get; init; }
    // public required string CardExpirationMonth { get; init; }
    // public required string CardExpirationYear { get; init; }
    // public required string CardCVV { get; init; }
}