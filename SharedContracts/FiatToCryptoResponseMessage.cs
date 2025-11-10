namespace SharedContracts
{
    public class FiatToCryptoResponseMessage
    {
        public Guid CorrelationId { get; set; }
        public DepositStatus Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
