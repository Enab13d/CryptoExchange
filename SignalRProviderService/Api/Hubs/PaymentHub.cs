using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedContracts;
using SignalRProviderService.Api.Interfaces;
using SignalRProviderService.Commands;
using SignalRProviderService.Infrastructure.Policies;

namespace SignalRProviderService.Api.Hubs;

[Authorize]
public class PaymentHub(IMediator mediator, ILogger<PaymentHub> logger) : Hub<IPaymentClient>
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<PaymentHub> _logger = logger;
    [Authorize(Policy = SignalRProviderAuthorizationPolicy.UserPolicy)]
    public async Task JoinHubGroup(string paymentId)
    {

        _logger.LogInformation("Added connection {ConnId} to group {Group}", Context.ConnectionId, paymentId);

        await Groups.AddToGroupAsync(Context.ConnectionId, paymentId);

        // await Clients.Group(paymentId).ReceiveConnectionMessage($"{Context.ConnectionId}");
        JoinHubGroupCommand command = new()
        {
            ConnectionId = Context.ConnectionId,
            PaymentId = paymentId
        };
        await _mediator.Send(command);

    }
    [Authorize(Policy = SignalRProviderAuthorizationPolicy.UserPolicy)]
    public async Task SendPaymentData(string paymentId)
    {
        _logger.LogInformation("Send Payment data command procedure invoked by client with paymentID {paymentId}", paymentId);
        PaymentDataRequestedCommand command = new()
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