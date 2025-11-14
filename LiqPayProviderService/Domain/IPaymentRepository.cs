using LiqPayProviderService.Domain.Constants;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Domain.SeedWork;

namespace LiqPayProviderService.Domain;

public interface IPaymentRepository : IRepository<Payment>
{
    Task UpdateById(Guid correlationId, PaymentStatus paymentStatus);


}