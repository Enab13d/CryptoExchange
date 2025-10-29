using LiqPayProviderService.Domain.SeedWork;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LiqPayProviderService.Domain.Entities;

public class Payment : IAggregateRoot
{
    public ObjectId Id { get; set; }
    public Guid OrderId { get; init; }
    public Guid CorrelationId { get; set; }
    public decimal Amount { get; set; }
    public string Fiat { get; set; } = string.Empty;
    public string Crypto { get; set; } = string.Empty;
    [BsonRepresentation(BsonType.String)]
    public PaymentStatus Status { get; set; }

}