namespace SharedContracts;


public record PaymentDataDTO()
{
    public string Data { get; init; } = string.Empty;

    public string Signature { get; init; } = string.Empty;
}