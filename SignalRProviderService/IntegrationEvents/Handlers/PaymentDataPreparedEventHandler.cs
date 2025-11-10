
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SharedContracts;
using SignalRProviderService.Api.Hubs;

namespace SignalRProviderService.IntegrationEvents.Handlers;


public class PaymentDataPreparedEventHandler(IHubContext<PaymentHub> hubContext, ILogger<PaymentDataPreparedEventHandler> logger) : IConsumer<PaymentDataDTO>
{
    private readonly IHubContext<PaymentHub> _hubContext = hubContext;
    private readonly ILogger<PaymentDataPreparedEventHandler> _logger = logger;
    public async Task Consume(ConsumeContext<PaymentDataDTO> context)
    {
        PaymentDataDTO paymentData = context.Message;

        //send data to specific client
        await _hubContext.Clients.Group(paymentData.PaymentId.ToString())
        .SendAsync("ReceivePaymentFormData", paymentData);

        _logger.LogInformation("Sent payment data to client for PaymentId: {paymentId}", paymentData.PaymentId);



    }
}