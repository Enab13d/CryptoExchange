//  1. Define  interface for IPaymentRepository LiqPayProviderService/Domain/IPaymentRepository.cs
// example
//https://github.com/dotnet-architecture/eShopOnContainersAI/tree/dev/src/Services/Ordering/Ordering.Infrastructure

using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Domain.SeedWork;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Constants;
using MongoDB.Bson;

namespace LiqPayProviderService.Domain;

public interface IPaymentRepository : IRepository<Payment>
{
    Payment Add(Payment payment);
    void Update(Payment payment);

    void UpdateById(Guid correlationId, PaymentStatus paymentStatus);
    Task<List<Payment>> FindAllAsync();
    Payment? FindByIdAsync(ObjectId id);
}