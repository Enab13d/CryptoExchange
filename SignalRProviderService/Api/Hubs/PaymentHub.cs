using Microsoft.AspNetCore.SignalR;

namespace SignalRProviderService.Api.Hubs;

public class PaymentHub : Hub
{
    public async Task RegisterPayment(Guid paymentId, CancellationToken cancellationToken)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, paymentId.ToString(), cancellationToken);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}