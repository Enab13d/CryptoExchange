using System.Security.Cryptography;
using System.Text;
using LiqPayProviderService.Api.Extensions;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.RequestParameters;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Responses;
using LiqPayProviderService.Infrastructure.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LiqPayProviderService.Infrastructure.Clients.LiqpayClient;

public class LiqpayClient : ILiqpayClient
{
    private readonly string _publicKey;
    private readonly string _privateKey;
    private readonly int _apiVersion;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _jsonSettings;

    private readonly ILogger<LiqpayClient> _logger;
    public bool IsCnbSandbox
    { get; set; }

    public LiqpayClient(IConfiguration configuration, HttpClient httpClient, ILogger<LiqpayClient> logger)
    {
        LiqPayClientOptions options = configuration
        .GetSection(nameof(LiqPayClientOptions))
        .Get<LiqPayClientOptions>() ?? throw new ArgumentNullException(nameof(configuration));

        _publicKey = options.PublicKey;
        _privateKey = options.PrivateKey;
        _apiVersion = options.LiqPayAPIVersion;
        _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        _httpClient = httpClient;
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
    private Dictionary<string, string> PrepareRequestData<T>(T requestParams) where T : ILiqpayBasicApiParams
    {
        var requestWithBaseApiParams = AttachBaseApiParams(requestParams);
        var requestWithSandboxParam = AttachSandboxParam(requestWithBaseApiParams);
        string json = SerializeToJson(requestWithSandboxParam);
        _logger.LogInformation("{json}", json);
        string data = json.ToBase64();
        _logger.LogInformation("{data}", data);
        Dictionary<string, string> apiData = new()
        {
            { "data", data },
            { "signature", CreateSignature(data)}
        };
        return apiData;
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

    public async Task<CardPaymentResponse> PayWithCardAsync(string path, CardPaymentRequest requestParams)
    {
        Dictionary<string, string> data = PrepareRequestData(requestParams);
        _logger.LogInformation("Private key: {PrivateKey}, public key: {PublicKey}", _privateKey, _publicKey);
        _logger.LogInformation("Signature: ${signature}", data["signature"]);
        using var content = new FormUrlEncodedContent(data);

        string relativePath = path?.TrimStart('/') ?? string.Empty;
        var fullUrl = new Uri(_httpClient.BaseAddress!, relativePath);
        _logger.LogInformation("Sending LiqPay request to URL: {Url}", fullUrl);
        var response = await _httpClient.PostAsync(path, content);
        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("LiqPay request to {Url} failed: {Status} - {Body}", fullUrl, response.StatusCode, body);
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("LiqPay request to {Url} sucess: {Status} - {Body}", fullUrl, response.StatusCode, body);
        response.EnsureSuccessStatusCode();
        string? json = await response.Content.ReadAsStringAsync() ?? throw new InvalidOperationException("response is null");
        return JsonConvert.DeserializeObject<CardPaymentResponse>(json)!;

    }
}