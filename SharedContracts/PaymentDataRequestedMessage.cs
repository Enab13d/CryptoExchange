namespace SharedContracts;

public class PaymentDataRequestedMessage
{
    public string PaymentId { get; set; } = string.Empty;

    public string ConnectionId { get; set; } = string.Empty;
}