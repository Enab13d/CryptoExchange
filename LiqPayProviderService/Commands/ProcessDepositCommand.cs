using MediatR;

public record ProcessDepositCommand : IRequest<bool>
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    // інші поля
}