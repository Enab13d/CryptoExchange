using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.RequestParameters;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Responses;

namespace LiqPayProviderService.Infrastructure.Clients.LiqpayClient;


public interface ILiqpayClient
{
    public bool IsCnbSandbox { get; set; }
    public Task<CardPaymentResponse> PayWithCardAsync(string path, CardPaymentRequest requestParams);

    public string CreateSignature(string base64EncodedData);
}