using System.Security.Cryptography;
using System.Text;
using LiqPayProviderService.Api.Extensions;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.RequestParameters;
using LiqPayProviderService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedContracts;

namespace LiqPayProviderService.Infrastructure.Clients.LiqpayClient;

public class LiqpayClient : ILiqpayClient
{
    private readonly string _publicKey;
    private readonly string _privateKey;
    private readonly int _apiVersion;
    private readonly JsonSerializerSettings _jsonSettings;

    private readonly ILogger<LiqpayClient> _logger;
    public bool IsCnbSandbox
    { get; set; }

    public LiqpayClient(IOptions<LiqPayClientOptions> options, ILogger<LiqpayClient> logger)
    {
        _publicKey = options.Value.PublicKey;
        _privateKey = options.Value.PrivateKey;
        _apiVersion = options.Value.LiqPayAPIVersion;
        _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        _logger = logger;

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
    public PaymentDataDTO PreparePaymentData<T>(T requestParams) where T : ILiqpayBasicApiParams
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
    // throws exception if API keys not set during client construction
    {
        if (string.IsNullOrEmpty(_publicKey))
        {
            throw new ArgumentNullException("publicKey is empty");
        }

        if (string.IsNullOrEmpty(_privateKey))
        {
            throw new ArgumentNullException("privateKey is empty");
        }
        if (_apiVersion <= 0)
        {
            throw new ArgumentNullException("_apiVersion is not set");
        }
    }

}