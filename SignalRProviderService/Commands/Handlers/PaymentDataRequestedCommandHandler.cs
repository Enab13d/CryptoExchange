using MassTransit;
using MediatR;
using SharedContracts;

namespace SignalRProviderService.Commands.Handlers;


public class PaymentDataRequestedCommandHandler(IPublishEndpoint publishEndpoint, ILogger<PaymentDataRequestedCommandHandler> logger) : IRequestHandler<PaymentDataRequestedCommand, PaymentDataRequestedMessage>
{

    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly ILogger<PaymentDataRequestedCommandHandler> _logger = logger;

    public async Task<PaymentDataRequestedMessage> Handle(PaymentDataRequestedCommand request, CancellationToken cancellationToken)
    {
        PaymentDataRequestedMessage message = new()
        {
            PaymentId = request.PaymentId,
            ConnectionId = request.ConnectionId
        };
        _logger.LogInformation("Publishing PaymentDataRequestedMessage with PaymentId {PaymentId} ConnectionId {ConnectionId}", message.PaymentId, message.ConnectionId);
        await _publishEndpoint.Publish(message, cancellationToken);
        return message;
    }
}