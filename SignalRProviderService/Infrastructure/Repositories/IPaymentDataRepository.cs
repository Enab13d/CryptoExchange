using SharedContracts;

namespace SignalRProviderService.Infrastructure.Repositories;

public interface IPaymentDataRepository
{
    public Task<PaymentDataDTO?> GetAsync(string key);

    public Task RemoveAsync(string key);

}