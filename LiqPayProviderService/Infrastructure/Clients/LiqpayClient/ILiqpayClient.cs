using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.RequestParameters;
// using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Responses;
using SharedContracts;

namespace LiqPayProviderService.Infrastructure.Clients.LiqpayClient;


public interface ILiqpayClient
{
    public bool IsCnbSandbox { get; set; }
    //only server-server integration
    // public Task<CardPaymentResponse> PayWithCardAsync(string path, CardPaymentRequest requestParams);
    public PaymentDataDTO PreparePaymentData<T>(T requestParams) where T : ILiqpayBasicApiParams;
    public string CreateSignature(string base64EncodedData);
}