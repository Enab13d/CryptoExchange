using System.Threading.Tasks;
using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Constants;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace LiqPayProviderService.Infrastructure.Repositories;


public class PaymentRepository(PaymentDbContext context) : IPaymentRepository
{
    private readonly PaymentDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<List<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Payments.ToListAsync(cancellationToken);
    }

    public Payment? GetById(ObjectId id, CancellationToken cancellationToken = default)
    {
        return _context.Payments.FirstOrDefault(e => e.Id == id);
    }
    public async Task<Payment?> GetByCorrelationId(Guid correlationId, CancellationToken cancellationToken = default)
    {
        return await _context.Payments.FirstOrDefaultAsync(e => e.CorrelationId == correlationId, cancellationToken);
    }
    public Payment Add(Payment entity, CancellationToken cancellationToken = default)
    {
        return _context.Payments.Add(entity).Entity;
    }
    public async Task Update(Payment payment)
    {
        Payment? existing = await _context.Payments.FirstOrDefaultAsync(e => e.CorrelationId == payment.CorrelationId)
        ?? throw new KeyNotFoundException($"Unable to update. Payment with correlationId {payment.CorrelationId} not exist.");

        existing.Status = payment.Status;
        existing.UpdatedAt = DateTime.Now;
    }

    public async Task UpdateById(Guid correlationId, PaymentStatus paymentStatus)
    {
        Payment? existing = await _context.Payments.FirstOrDefaultAsync(e => e.CorrelationId == correlationId)
         ?? throw new KeyNotFoundException($"Unable to update. Payment with correlationId {correlationId} not exist."); ;
        if (existing is null) return;

        existing.Status = paymentStatus;
        existing.UpdatedAt = DateTime.Now;
    }

    public void Remove(Payment entity)
    {
        _context.Payments.Remove(entity);
    }


}