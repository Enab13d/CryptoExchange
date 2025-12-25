using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRProviderService.Api.Extensions;
using SignalRProviderService.Api.Interfaces;
using SignalRProviderService.Infrastructure.Policies;
using SignalRProviderService.Infrastructure.Repositories;

namespace SignalRProviderService.Api.Hubs;

[Authorize(Policy = SignalRProviderAuthorizationPolicy.UserPolicy)]
public class PaymentHub(ILogger<PaymentHub> logger, IPaymentDataRepository paymentDataRepository) : Hub<IPaymentClient>
{

    private readonly ILogger<PaymentHub> _logger = logger;
    private readonly IPaymentDataRepository _paymentDataRepository = paymentDataRepository;
    public async Task JoinHubGroup(string paymentId)
    {

        _logger.LogInformation("Added connection {ConnId} to group {Group}", Context.ConnectionId, paymentId);

        await Groups.AddToGroupAsync(Context.ConnectionId, paymentId);

        // await Clients.Group(paymentId).ReceiveConnectionMessage($"{Context.ConnectionId}");


    }
    public async Task SendPaymentData(string paymentId)
    {
        _logger.LogInformation("Send Payment data command procedure invoked by client with paymentID {paymentId}", paymentId);


        var paymentData = await _paymentDataRepository.GetAsync(paymentId) ?? throw new InvalidOperationException($"Payment data with cacheKey {paymentId} not found");
        //then extract payment data and 
        await Clients.Group(paymentId)
        .ReceivePaymentFormData(paymentData);
        _logger.LogInformation("Sent payment data to client group with paymentId {id}", paymentId);
        await _paymentDataRepository.RemoveAsync(paymentId);


    }

    public override Task OnConnectedAsync()
    {
        var user = Context.UserFromClaims();
        if (user is not null) _logger.LogInformation("User {id} connected to hub", user.UserId);
        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation(
            "Connection {ConnId} disconnected. Reason: {Reason}",
            Context.ConnectionId, exception?.Message
        );
        await base.OnDisconnectedAsync(exception);
    }
}