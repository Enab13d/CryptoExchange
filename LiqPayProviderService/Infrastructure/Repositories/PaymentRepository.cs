
//  2. Implement payment repository LiqPayProviderService/Persistence/PaymentRepository.cs

// example
// https://github.com/dotnet-architecture/eShopOnContainersAI/blob/dev/src/Services/Ordering/Ordering.Infrastructure/Repositories/BuyerRepository.cs
using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Constants;
using LiqPayProviderService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace LiqPayProviderService.Infrastructure.Repositories;


public class PaymentRepository(PaymentDbContext context) : IPaymentRepository
{

    private readonly PaymentDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IUnitOfWork UnitOfWork { get => _context; }

    public Payment Add(Payment payment)
    {
        return _context.Payments.Add(payment).Entity;
    }

    public async Task<List<Payment>> FindAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public Payment? FindByIdAsync(ObjectId id)
    {
        return _context.Payments.FirstOrDefault(e => e.Id == id);
    }

    public void Update(Payment payment)
    {
        Payment? existing = _context.Payments.FirstOrDefault(e => e.CorrelationId == payment.CorrelationId);
        if (existing is null) return;

        existing.Status = payment.Status;
    }

    public void UpdateById(Guid correlationId, PaymentStatus paymentStatus)
    {
        Payment? existing = _context.Payments.FirstOrDefault(e => e.CorrelationId == correlationId);
        if (existing is null) return;

        existing.Status = paymentStatus;
    }
}