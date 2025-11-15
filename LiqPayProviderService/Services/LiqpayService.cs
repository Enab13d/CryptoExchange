using LiqPayProviderService.Api.Extensions;
using SharedContracts;
using System.Security.Cryptography;
using System.Text;
using LiqPayProviderService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LiqPayProviderService.Domain.RequestParameters;

namespace LiqPayProviderService.Services;

public class LiqPayService : ILiqpayService
{
    private readonly string _publicKey;
    private readonly string _privateKey;
    private readonly int _apiVersion;
    private readonly string _webhookURL;
    private readonly JsonSerializerSettings _jsonSettings;
    private readonly ILogger<LiqPayService> _logger;
    public bool IsCnbSandbox
    { get; set; }

    public LiqPayService(IOptions<LiqPayOptions> liqpayOptions, IOptions<WebhookOptions> webhookOptions, ILogger<LiqPayService> logger)
    {
        _publicKey = liqpayOptions.Value.PublicKey;
        _privateKey = liqpayOptions.Value.PrivateKey;
        _apiVersion = liqpayOptions.Value.LiqPayAPIVersion;
        _webhookURL = webhookOptions.Value.URL;
        _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        _logger = logger;
        IsCnbSandbox = true;
        CheckConstructionPrerequisites();
    }


    public T AttachBaseApiParams<T>(T requestParams) where T : ILiqpayBasicApiParams
    {
        requestParams.PublicKey = _publicKey;
        requestParams.Version = _apiVersion;
        return requestParams;
    }
    public T AttachSandboxParam<T>(T requestParams) where T : ILiqpayBasicApiParams
    {
        requestParams.IsSandbox = IsCnbSandbox;
        return requestParams;
    }
    private string SerializeToJson<T>(T requestParams)
    {
        ArgumentNullException.ThrowIfNull(requestParams);
        var json = JObject.FromObject(requestParams, new JsonSerializer { NullValueHandling = _jsonSettings.NullValueHandling });
        return json.ToString();
    }
    public PaymentDataDTO PreparePaymentFormData<T>(T requestParams) where T : ILiqpayBasicApiParams
    {
        var requestWithBaseApiParams = AttachBaseApiParams(requestParams);
        var requestWithSandboxParam = AttachSandboxParam(requestWithBaseApiParams);
        string json = SerializeToJson(requestWithSandboxParam);
        string data = json.ToBase64();
        return new PaymentDataDTO
        {
            Data = data,
            Signature = CreateSignature(data)

        };
    }
    private static string StrToSign(string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        var bytes = Encoding.UTF8.GetBytes(str);
        var hashBytes = SHA3_256.HashData(bytes); // raw binary hash
        return Convert.ToBase64String(hashBytes);  // Base64 of raw hash
    }

    public string CreateSignature(string base64EncodedData) => StrToSign(_privateKey + base64EncodedData + _privateKey);

    private void CheckConstructionPrerequisites()
    // throws exception if API keys and other necessary fields not set during client construction
    {
        if (string.IsNullOrEmpty(_publicKey))
        {
            throw new ArgumentNullException("publicKey is empty");
        }

        if (string.IsNullOrEmpty(_privateKey))
        {
            throw new ArgumentNullException("privateKey is empty");
        }

        if (string.IsNullOrEmpty(_webhookURL))
        {
            throw new ArgumentNullException("webhookURL is empty");
        }
        if (_apiVersion <= 0)
        {
            throw new ArgumentNullException("_apiVersion is not set");
        }
    }


    public PaymentDataDTO PreparePaymentData(DepositDTO deposit)
    {
        CardPaymentRequest request = deposit.ToCardPaymentRequest();

        //assign webhook url
        request.ServerUrl = $"{_webhookURL}/payment/callback";
        PaymentDataDTO data = PreparePaymentFormData(request);
        return data;
    }
}