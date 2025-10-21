using MediatR;

public record ProcessDepositCommand : IRequest<bool>
{
    public Guid CorrelationId { get; init; }
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    // інші поля
}