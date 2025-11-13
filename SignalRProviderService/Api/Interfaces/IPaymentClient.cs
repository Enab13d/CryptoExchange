using SharedContracts;

namespace SignalRProviderService.Api.Interfaces;


public interface IPaymentClient
{
    Task ReceivePaymentDataMessage(PaymentDataDTO paymentData);
    Task ReceiveConnectionMessage(string connectionId);
}