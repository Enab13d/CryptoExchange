using LiqPayProviderService.Api.Extensions;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.RequestParameters;
using SharedContracts;

namespace LiqPayProviderService.Services;

public class LiqPayService : ILiqpayService
{

    public LiqPayService(ILiqpayClient client, IConfiguration configuration)
    {
        _client = client;
        // set sanbox mode
        _client.IsCnbSandbox = true;
        _configuration = configuration;

    }
    private readonly ILiqpayClient _client;
    private readonly IConfiguration _configuration;

    public PaymentDataDTO PreparePaymentData(DepositDTO deposit)
    {
        //implement request to liqpay api via httpClient
        CardPaymentRequest request = deposit.ToCardPaymentRequest();


        //assign webhook url
        request.ServerUrl = _configuration["Webhook:URL"] + "/api/payment/callback";
        PaymentDataDTO data = _client.PreparePaymentData(request);
        return data;
    }
}