using SharedContracts;

namespace LiqPayProviderService.Services;


public interface ILiqpayService
{
    PaymentDataDTO PreparePaymentData(DepositDTO deposit);
    string CreateSignature(string base64EncodedData);
}