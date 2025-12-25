
using MediatR;
using MassTransit;
using SharedContracts;
using SignalRProviderService.Api.Commands;

namespace SignalRProviderService.Infrastructure.IntegrationEvents.Handlers;

public class PaymentDataPreparedEventHandler(IMediator mediator, ILogger<PaymentDataPreparedEventHandler> logger) : IConsumer<PaymentDataDTO>
{
    private readonly IMediator _mediator = mediator;

    private readonly ILogger<PaymentDataPreparedEventHandler> _logger = logger;
    public async Task Consume(ConsumeContext<PaymentDataDTO> context)
    {
        PaymentDataDTO message = context.Message;

        SendPaymentDataCommand command = new()
        {
            CorrelationId = message.CorrelationId,
            PaymentId = message.PaymentId,
            Data = message.Data,
            Signature = message.Signature
        };
        await _mediator.Send(command);
        _logger.LogInformation("Send ProcessCryptoPayoutCommand with CorrelationId {id}", command.CorrelationId);
    }
}