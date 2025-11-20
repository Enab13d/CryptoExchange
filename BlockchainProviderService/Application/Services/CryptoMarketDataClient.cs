using System.Threading.Tasks;
using System.Web;
using BlockchainProviderService.Application.Mappers;
using BlockchainProviderService.Domain.Entities;
using SharedContracts.Constants;

namespace BlockchainProviderService.Application.Services;

public class CryptoMarketDataClient(HttpClient httpClient, ICurrencyMapper currencyMapper) : ICryptoMarketDataClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ICurrencyMapper _currencyMapper = currencyMapper;
    public async Task<decimal> ConvertCryptoToFiatAsync(Crypto crypto, Currency fiat, decimal amount)
    {
        int fromId = _currencyMapper.CryptoToConversionId(crypto);
        int toId = _currencyMapper.FiatToConverstionId(fiat);

        PriceConversionResponse response = await SendPriceConversionRequest(fromId, toId, amount);
        return ExtractCurrencyValue(response);

    }

    public async Task<decimal> ConvertFiatToCryptoAsync(Currency fiat, Crypto crypto, decimal amount)
    {
        int toId = _currencyMapper.CryptoToConversionId(crypto);
        int fromId = _currencyMapper.FiatToConverstionId(fiat);

        PriceConversionResponse response = await SendPriceConversionRequest(fromId, toId, amount);
        return ExtractCurrencyValue(response);
    }

    private async Task<PriceConversionResponse> SendPriceConversionRequest(int fromId, int toId, decimal amount)
    {
        string url = BuildQueryURL(fromId, toId, amount);
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);
        httpResponse.EnsureSuccessStatusCode();
        PriceConversionResponse? response = await httpResponse.Content.ReadFromJsonAsync<PriceConversionResponse>()
        ?? throw new Exception("Empty response from CoinMarketCap API");
        return response;

    }
    private string BuildQueryURL(int fromId, int toId, decimal amount)
    {
        UriBuilder builder = new(new Uri(_httpClient.BaseAddress!, "v2/tools/price-conversion"));
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["id"] = fromId.ToString();
        query["amount"] = amount.ToString();
        query["convert_id"] = toId.ToString();

        builder.Query = query.ToString();
        return builder.ToString();
    }
    private static decimal ExtractCurrencyValue(PriceConversionResponse response) => response.Data.Quote.First().Value.Price;

}