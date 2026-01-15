using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Constants;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SharedContracts;

namespace LiqPayProviderService.Commands.Handlers;

public class ProcessDepositCommandHandler(
  ILogger<ProcessDepositCommandHandler> logger,
  IPaymentRepository paymentRepository,
  ILiqpayService liqpayService,
  IUnitOfWork unitOfWork,
  IPublishEndpoint publishEndpoint,
  IDistributedCache cache
      ) : IRequestHandler<ProcessDepositCommand, PaymentDataDTO>
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILiqpayService _liqpayService = liqpayService;
    private readonly ILogger<ProcessDepositCommandHandler> _logger = logger;
    private readonly IDistributedCache _cache = cache;

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
            UserId = command.UserId

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

        var cacheKey = $"payment-form:{paymentData.PaymentId}";
        var json = System.Text.Json.JsonSerializer.Serialize(paymentData);
        //save payment data to cache so that signalr can retrieve it later
        await _cache.SetStringAsync(
            cacheKey,
             json, new DistributedCacheEntryOptions
             {
                 //set ttl to 5 mins
                 AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
             },
             cancellationToken
            );

        // publish payment data to workflow
        await _publishEndpoint.Publish(paymentData, cancellationToken);

        // Process deposit logic here
        _logger.LogInformation("Prepared LiqPay checkout data for CorrelationId: {CorrelationId}", command.CorrelationId);


        return paymentData;
    }
}