
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SharedContracts;
using SignalRProviderService.Api.Hubs;
using SignalRProviderService.Api.Interfaces;

namespace SignalRProviderService.Api.Commands.Handlers;


public class PaymentDataPreparedEventHandler(IHubContext<PaymentHub, IPaymentClient> hubContext, ILogger<PaymentDataPreparedEventHandler> logger) : IConsumer<PaymentDataDTO>
{
    private readonly IHubContext<PaymentHub, IPaymentClient> _hubContext = hubContext;
    private readonly ILogger<PaymentDataPreparedEventHandler> _logger = logger;
    public async Task Consume(ConsumeContext<PaymentDataDTO> context)
    {
        PaymentDataDTO paymentData = context.Message;

        //send data to specific client
        //message contain the method name "ReceivePaymentFormData" that should be invoked on the client
        //and arguments (paymentData) that client should pass into those method
        await _hubContext.Clients.Group(paymentData.PaymentId.ToString())
        .ReceivePaymentFormData(paymentData);

        _logger.LogInformation("Sent payment data to client with PaymentId: {paymentId}, connection ID", paymentData.PaymentId.ToString());



    }
}