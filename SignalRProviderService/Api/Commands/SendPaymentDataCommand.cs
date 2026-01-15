using MediatR;
using SharedContracts;

namespace SignalRProviderService.Api.Commands;


public class SendPaymentDataCommand : IRequest<PaymentDataDTO>
{
    public Guid CorrelationId { get; set; }
    public Guid PaymentId { get; set; }
    public string Data { get; init; } = string.Empty;
    public string Signature { get; init; } = string.Empty;
}