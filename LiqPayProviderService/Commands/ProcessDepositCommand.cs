using MediatR;
using SharedContracts;

namespace LiqPayProviderService.Commands;

public record ProcessDepositCommand : IRequest<PaymentDataDTO>
{
    public Guid CorrelationId { get; init; }
    public Guid Id { get; init; }
    public string Fiat { get; set; } = default!;
    public string Crypto { get; set; } = default!;
    public decimal Amount { get; init; }
    // інші поля
    public string Description { get; init; } = string.Empty;
    public Guid OrderId { get; init; }
    public string Phone { get; init; } = string.Empty;
}
