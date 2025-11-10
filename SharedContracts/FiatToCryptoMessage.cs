namespace SharedContracts
{
    public class FiatToCryptoMessage
    {
        public Guid CorrelationId { get; set; }
        public string Fiat { get; set; } = default!;
        public string Crypto { get; set; } = default!;
        public decimal Amount { get; set; }
        public string Currency { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; init; }
        public string Phone { get; init; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public FiatToCryptoResponseMessage Response { get; set; } = default!;
    }
}
