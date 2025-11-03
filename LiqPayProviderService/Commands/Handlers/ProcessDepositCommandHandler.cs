using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Constants;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Responses;
using LiqPayProviderService.Services;
using MediatR;
using SharedContracts;

namespace LiqPayProviderService.Commands.Handlers;

public class ProcessDepositCommandHandler : IRequestHandler<ProcessDepositCommand, bool>
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

    public async Task<bool> Handle(ProcessDepositCommand command, CancellationToken cancellationToken)
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
            Card = command.Card,
            CardExpirationMonth = command.CardExpirationMonth,
            CardExpirationYear = command.CardExpirationYear,
            CardCVV = command.CardCVV
        };

        // invoke httpClient 
        CardPaymentResponse response = await _liqpayService.Deposit(deposit);


        // invoke deposit command


        // Process deposit logic here
        _logger.LogInformation("Processing deposit for Command Id: {CommandId} with Amount: {Amount}. CORRELATION ID {CorrelationId}", command.Id, command.Amount, command.CorrelationId);


        return true;
    }
}