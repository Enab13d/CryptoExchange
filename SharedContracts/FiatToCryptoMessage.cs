using SharedContracts.Constants;

namespace SharedContracts
{
    public class FiatToCryptoMessage
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
        public DateTime CreatedAt { get; set; }
        public PaymentDataDTO PaymentData { get; set; } = default!;
        public WebsocketConnectionMessage WebsocketConnectionMessage { get; set; } = default!;
        public FiatToCryptoResponseMessage Response { get; set; } = default!;

        public PreparedFormDataMessage PreparedFormDataMessage { get; set; } = default!;
    }
}
