using MediatR;
using SharedContracts;

namespace SignalRProviderService.Commands
{
    public class PaymentDataRequestedCommand : IRequest<PaymentDataRequestedMessage>
    {
        public string PaymentId { get; set; } = string.Empty;

        public string ConnectionId { get; set; } = string.Empty;
    }
}