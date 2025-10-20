public record DepositRequestedEvent
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    // інші необхідні поля
}