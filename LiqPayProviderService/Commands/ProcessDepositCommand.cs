using MediatR;
using SharedContracts;
using SharedContracts.Constants;

namespace LiqPayProviderService.Commands;

public record ProcessDepositCommand : IRequest<PaymentDataDTO>
{
    public Guid CorrelationId { get; init; }
    public Guid Id { get; init; }

    public Guid PaymentId { get; init; }
    public Currency Fiat { get; set; }
    public Crypto Crypto { get; set; }
    public decimal Amount { get; init; }
    // інші поля

    public string WalletAddress { get; set; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid OrderId { get; init; }
    public string Phone { get; init; } = string.Empty;
}
