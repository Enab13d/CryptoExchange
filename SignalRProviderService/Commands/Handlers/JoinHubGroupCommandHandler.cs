using MassTransit;
using MediatR;
using SharedContracts;

namespace SignalRProviderService.Commands.Handlers;


public class JoinHubGroupCommandHandler(ISendEndpointProvider sendEndpointProvider, ILogger<JoinHubGroupCommandHandler> logger) : IRequestHandler<JoinHubGroupCommand, WebsocketConnectionMessage>
{

    private readonly ISendEndpointProvider _sendEndpointProvider = sendEndpointProvider;
    private readonly ILogger<JoinHubGroupCommandHandler> _logger = logger;

    public async Task<WebsocketConnectionMessage> Handle(JoinHubGroupCommand request, CancellationToken cancellationToken)
    {
        WebsocketConnectionMessage message = new()
        {
            PaymentId = request.PaymentId,
            ConnectionId = request.ConnectionId
        };
        _logger.LogInformation("Sending WebsocketConnectionMessage with PaymentId {PaymentId} ConnectionId {ConnectionId}", message.PaymentId, message.ConnectionId);
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:websocket-connection-established"));
        await endpoint.Send(message, cancellationToken);
        return message;
    }
}