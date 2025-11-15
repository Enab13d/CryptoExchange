using MediatR;
using SharedContracts;

namespace SignalRProviderService.Commands
{
    public class JoinHubGroupCommand : IRequest<WebsocketConnectionMessage>
    {
        public string PaymentId { get; set; } = string.Empty;

        public string ConnectionId { get; set; } = string.Empty;
    }
}