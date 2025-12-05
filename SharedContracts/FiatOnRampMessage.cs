using SharedContracts.Constants;

namespace SharedContracts
{
    public class FiatOnRampMessage
    {
        public Guid CorrelationId { get; set; }
        public Currency Fiat { get; set; }
        public Crypto Crypto { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; init; }
        public string Description { get; init; } = string.Empty;
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; init; }
        public string Phone { get; init; } = string.Empty;
        public string WalletAddress { get; init; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public User User { get; set; } = null!;
        public PaymentDataDTO PaymentData { get; set; } = default!;
        public WebsocketConnectionMessage WebsocketConnectionMessage { get; set; } = default!;
        public PreparedFormDataMessage PreparedFormDataMessage { get; set; } = default!;
    }
}
