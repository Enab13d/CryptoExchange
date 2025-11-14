using LiqPayProviderService.Domain.Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SharedContracts.Constants;

namespace LiqPayProviderService.Domain.Entities;

public class Payment
{
    public ObjectId Id { get; set; }
    public Guid OrderId { get; init; }
    public Guid CorrelationId { get; set; }
    public Guid UserId { get; set; }

    public Guid PaymentId { get; init; }
    public decimal Amount { get; set; }
    public Currency Fiat { get; set; }
    public Crypto Crypto { get; set; }
    [BsonRepresentation(BsonType.String)]
    public PaymentStatus Status { get; set; }

    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; init; }

}