using MediatR;
using SharedContracts.Constants;

namespace BlockchainProviderService.Application.Commands;


public class ProcessCryptoPayoutCommand : IRequest<bool>
{
    public Guid CorrelationId { get; set; }
    public Currency Fiat { get; set; }
    public Crypto Crypto { get; set; }
    public decimal Amount { get; set; }
    public string WalletAddress { get; init; } = string.Empty;
}