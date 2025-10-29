using LiqPayProviderService.Infrastructure.Clients.LiqpayClient;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Mappers;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.RequestParameters;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Responses;
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

    public async Task<CardPaymentResponse> Deposit(DepositDTO deposit)
    {
        //implement request to liqpay api via httpClient
        CardPaymentRequest request = deposit.ToCardPaymentRequest();
        //assign webhook url
        request.ServerUrl = _configuration["Webhook:URL"] + "/api/payment/callback";
        CardPaymentResponse response = await _client.PayWithCardAsync("request", request);
        return response;
    }
}