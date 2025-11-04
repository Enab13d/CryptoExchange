namespace SharedContracts
{
    public class FiatToCryptoResponseMessage
    {
        public Guid CorrelationId { get; set; }
        public string Status { get; set; } = default!;
        public string? CreateDate { get; set; }
        public string? UpdateDate { get; set; }
    }
}
