using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Constants;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Services;
using MassTransit;
using MediatR;
using SharedContracts;

namespace LiqPayProviderService.Commands.Handlers;

public class ProcessDepositCommandHandler : IRequestHandler<ProcessDepositCommand, PaymentDataDTO>
{

    private readonly IPaymentRepository _paymentRepository;

    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILiqpayService _liqpayService;
    private readonly ILogger<ProcessDepositCommandHandler> _logger;

    public ProcessDepositCommandHandler
    (ILogger<ProcessDepositCommandHandler> logger,
      IPaymentRepository paymentRepository,
      ILiqpayService liqpayService,
      IUnitOfWork unitOfWork,
      IPublishEndpoint publishEndpoint

      )
    {
        _logger = logger;
        // Inject IRepository interface +
        _paymentRepository = paymentRepository;
        // inject http handler +
        _liqpayService = liqpayService;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<PaymentDataDTO> Handle(ProcessDepositCommand command, CancellationToken cancellationToken)
    {
        //save object with correlation ID to db with repository pattern 
        DateTime timestamp = DateTime.Now;
        _paymentRepository.Add(new Payment()
        {
            OrderId = command.OrderId,
            CorrelationId = command.CorrelationId,
            PaymentId = command.PaymentId,
            Amount = command.Amount,
            Fiat = command.Fiat,
            Crypto = command.Crypto,
            Status = PaymentStatus.Processing,
            CreatedAt = timestamp,
            UpdatedAt = timestamp,
            WalletAddress = command.WalletAddress,
            User = command.User

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
            Crypto = command.Crypto,
            PaymentId = command.PaymentId,
            WalletAddress = command.WalletAddress
        };

        PaymentDataDTO paymentData = _liqpayService.PreparePaymentData(deposit);
        paymentData.CorrelationId = command.CorrelationId;
        paymentData.PaymentId = command.PaymentId;
        // publish payment data to workflow
        await _publishEndpoint.Publish(paymentData, cancellationToken);

        // Process deposit logic here
        _logger.LogInformation("Prepared LiqPay checkout data for CorrelationId: {CorrelationId}", command.CorrelationId);


        return paymentData;
    }
}