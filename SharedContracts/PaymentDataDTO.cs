namespace SharedContracts;


public record PaymentDataDTO()
{
    public Guid CorrelationId { get; set; }
    public string Data { get; init; } = string.Empty;

    public string Signature { get; init; } = string.Empty;
}