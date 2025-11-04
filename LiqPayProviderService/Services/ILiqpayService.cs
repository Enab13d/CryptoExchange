
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Responses;
using SharedContracts;

namespace LiqPayProviderService.Services;


public interface ILiqpayService
{
    PaymentDataDTO PreparePaymentData(DepositDTO deposit);
}