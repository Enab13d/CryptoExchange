using MassTransit;
using MediatR;
using SharedContracts;

namespace SignalRProviderService.Commands.Handlers;


public class PaymentDataRequestedCommandHandler(ISendEndpointProvider sendEndpointProvider, ILogger<PaymentDataRequestedCommandHandler> logger) : IRequestHandler<PaymentDataRequestedCommand, PaymentDataRequestedMessage>
{
    private readonly ISendEndpointProvider _sendEndpointProvider = sendEndpointProvider;
    private readonly ILogger<PaymentDataRequestedCommandHandler> _logger = logger;

    public async Task<PaymentDataRequestedMessage> Handle(PaymentDataRequestedCommand request, CancellationToken cancellationToken)
    {
        PaymentDataRequestedMessage message = new()
        {
            PaymentId = request.PaymentId,
            ConnectionId = request.ConnectionId
        };
        _logger.LogInformation("Sending PaymentDataRequestedMessage with PaymentId {PaymentId} ConnectionId {ConnectionId}", message.PaymentId, message.ConnectionId);
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:payment-data-requested"));
        await endpoint.Send(message, cancellationToken);
        return message;
    }
}