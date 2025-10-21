namespace SharedContracts
{
    public class FiatToCryptoMessage
    {
        public Guid CorrelationId { get; set; }
        public string Fiat { get; set; } = default!;
        public string Crypto { get; set; } = default!;
        public int Amount { get; set; }
        public FiatToCryptoResponseMessage Response { get; set; } = default!;
    }
}
