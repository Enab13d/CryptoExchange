using MediatR;
using Microsoft.AspNetCore.SignalR;
using SharedContracts;
using SignalRProviderService.Api.Interfaces;
using SignalRProviderService.Commands;

namespace SignalRProviderService.Api.Hubs;

public class PaymentHub(IMediator mediator, ILogger<PaymentHub> logger) : Hub<IPaymentClient>
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<PaymentHub> _logger = logger;
    public async Task JoinHubGroup(string paymentId)
    {

            _logger.LogInformation("Added connection {ConnId} to group {Group}", Context.ConnectionId, paymentId);

            // await Clients.Group(paymentId).ReceiveConnectionMessage($"{Context.ConnectionId}");
            JoinHubGroupCommand command = new()
            {
                ConnectionId = Context.ConnectionId,
                PaymentId = paymentId
            };
            await _mediator.Send(command);

    }



    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}