using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using SharedContracts;

namespace SignalRProviderService.Infrastructure.Repositories;

public class PaymentDataRepository(IDistributedCache cache) : IPaymentDataRepository

{
    private readonly IDistributedCache _cache = cache;
    public async Task<PaymentDataDTO?> GetAsync(string key)
    {
        var json = await _cache.GetStringAsync($"payment-form:{key}");
        if (string.IsNullOrWhiteSpace(json)) return null;
        return JsonSerializer.Deserialize<PaymentDataDTO>(json);

    }

    public async Task RemoveAsync(string key) => await _cache.RemoveAsync($"payment-form:{key}");
}