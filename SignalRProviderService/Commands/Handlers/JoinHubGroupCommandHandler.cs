using MassTransit;
using MediatR;
using SharedContracts;

namespace SignalRProviderService.Commands.Handlers;


public class JoinHubGroupCommandHandler(IPublishEndpoint publishEndpoint, ILogger<JoinHubGroupCommandHandler> logger) : IRequestHandler<JoinHubGroupCommand, WebsocketConnectionMessage>
{

    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly ILogger<JoinHubGroupCommandHandler> _logger = logger;

    public async Task<WebsocketConnectionMessage> Handle(JoinHubGroupCommand request, CancellationToken cancellationToken)
    {
        WebsocketConnectionMessage message = new()
        {
            PaymentId = request.PaymentId,
            ConnectionId = request.ConnectionId
        };
        _logger.LogInformation("Publishing WebsocketConnectionMessage with PaymentId {PaymentId} ConnectionId {ConnectionId}", message.PaymentId, message.ConnectionId);
        await _publishEndpoint.Publish(message, cancellationToken);
        return message;
    }
}