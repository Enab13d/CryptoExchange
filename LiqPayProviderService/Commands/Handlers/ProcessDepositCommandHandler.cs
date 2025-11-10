using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Constants;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Responses;
using LiqPayProviderService.Services;
using MediatR;
using SharedContracts;

namespace LiqPayProviderService.Commands.Handlers;

public class ProcessDepositCommandHandler : IRequestHandler<ProcessDepositCommand, PaymentDataDTO>
{

    private readonly IPaymentRepository _paymentRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILiqpayService _liqpayService;
    private readonly ILogger<ProcessDepositCommandHandler> _logger;

    public ProcessDepositCommandHandler
    (ILogger<ProcessDepositCommandHandler> logger,
      IPaymentRepository paymentRepository,
      ILiqpayService liqpayService,
      IUnitOfWork unitOfWork

      )
    {
        _logger = logger;
        // Inject IRepository interface +
        _paymentRepository = paymentRepository;
        // inject http handler +
        _liqpayService = liqpayService;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaymentDataDTO> Handle(ProcessDepositCommand command, CancellationToken cancellationToken)
    {
        //save object with correlation ID to db with repository pattern 
        _paymentRepository.Add(new Payment()
        {
            OrderId = command.OrderId,
            CorrelationId = command.CorrelationId,
            Amount = command.Amount,
            Fiat = command.Fiat,
            Crypto = command.Crypto,
            Status = PaymentStatus.Processing

        }, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        DepositDTO deposit = new()
        {
            CorrelationId = command.CorrelationId,
            Amount = command.Amount,
            Currency = command.Fiat,
            Description = command.Description,
            OrderId = command.OrderId.ToString(),
            Phone = command.Phone,
        };

        PaymentDataDTO paymentData = _liqpayService.PreparePaymentData(deposit);
        // publish payment data to workflow


        // Process deposit logic here
        _logger.LogInformation("Prepared LiqPay checkout data for CorrelationId: {CorrelationId}", command.CorrelationId);


        return paymentData;
    }
}