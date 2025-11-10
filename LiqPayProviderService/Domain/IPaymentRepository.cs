//  1. Define  interface for IPaymentRepository LiqPayProviderService/Domain/IPaymentRepository.cs
// example
//https://github.com/dotnet-architecture/eShopOnContainersAI/tree/dev/src/Services/Ordering/Ordering.Infrastructure

using LiqPayProviderService.Domain.Constants;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Domain.SeedWork;

namespace LiqPayProviderService.Domain;

public interface IPaymentRepository : IRepository<Payment>
{
    void UpdateById(Guid correlationId, PaymentStatus paymentStatus);


}