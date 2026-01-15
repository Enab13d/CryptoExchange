using MediatR;
using SharedContracts;
using SharedContracts.Constants;

namespace LiqPayProviderService.Commands;

public record ProcessDepositCommand : IRequest<PaymentDataDTO>
{
        public Guid CorrelationId { get; init; }
        public Guid PaymentId { get; init; }
        public Guid OrderId { get; init; }
        public decimal Amount { get; init; }
        public Currency Fiat { get; init; }
        public Crypto Crypto { get; init; }

        public string Description { get; set; } = string.Empty;

        public string WalletAddress { get; init; } = string.Empty;
        public string UserId { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
}
