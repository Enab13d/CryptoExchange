using LiqPayProviderService.Domain.Constants;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Domain.SeedWork;

namespace LiqPayProviderService.Domain;

public interface IPaymentRepository : IRepository<Payment>
{
    Task UpdateById(Guid correlationId, PaymentStatus paymentStatus);

    Task<Payment?> GetByCorrelationId(Guid correlationId, CancellationToken cancellationToken = default);


}