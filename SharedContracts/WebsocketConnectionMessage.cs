namespace SharedContracts;

public class WebsocketConnectionMessage
{
    public string PaymentId { get; set; } = string.Empty;

    public string ConnectionId { get; set; } = string.Empty;
}