
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SharedContracts;
using SignalRProviderService.Api.Hubs;

namespace SignalRProviderService.IntegrationEvents.Handlers;


public class PaymentDataRequestedEventHandler(IHubContext<PaymentHub> hubContext, ILogger<PaymentDataRequestedEventHandler> logger) : IConsumer<PreparedFormDataMessage>
{
    private readonly IHubContext<PaymentHub> _hubContext = hubContext;
    private readonly ILogger<PaymentDataRequestedEventHandler> _logger = logger;
    public async Task Consume(ConsumeContext<PreparedFormDataMessage> context)
    {
        PreparedFormDataMessage paymentData = context.Message;

        //send data to specific client
        //message contain the method name "ReceivePaymentFormData" that should be invoked on the client
        //and arguments (paymentData) that client should pass into those method
        await _hubContext.Clients.Group(paymentData.PaymentId.ToString())
        .SendAsync("ReceivePaymentFormData", paymentData);

        _logger.LogInformation("Sent payment data to client with PaymentId: {paymentId}", paymentData.PaymentId.ToString());



    }
}