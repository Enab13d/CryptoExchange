using LiqPayProviderService.Domain.Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LiqPayProviderService.Domain.Entities;

public class Payment
{
    public ObjectId Id { get; set; }
    public Guid OrderId { get; init; }
    public Guid CorrelationId { get; set; }
    public Guid UserId { get; set; }

    public Guid PaymentId { get; init; }
    public decimal Amount { get; set; }
    public string Fiat { get; set; } = string.Empty;
    public string Crypto { get; set; } = string.Empty;
    [BsonRepresentation(BsonType.String)]
    public PaymentStatus Status { get; set; }

    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; init; }

}