using SharedContracts;

namespace SignalRProviderService.Api.Interfaces;


public interface IPaymentClient
{
    Task ReceivePaymentFormData(PreparedFormDataMessage paymentData);
}