using System.Text;
using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedContracts;


namespace LiqPayProviderService.Api.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class PaymentController(IPublishEndpoint publishEndpoint, IPaymentRepository paymentRepository, IUnitOfWork unitOfWork, ILiqpayService liqpayService, ILogger<PaymentController> logger) : ControllerBase
{

    private readonly ILiqpayService _liqpayService = liqpayService;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly ILogger<PaymentController> _logger = logger;
    [HttpPost("callback")]
    public async Task<IActionResult> HandleLiqpayCallback(CancellationToken cancellationToken)
    {
        IFormCollection form = await Request.ReadFormAsync(cancellationToken);

        var data = form["data"].ToString();
        var signature = form["signature"].ToString();
        _logger.LogInformation("data: {data}. signature: {signature}", data, signature);
        bool isValidRequest = _liqpayService.CreateSignature(data) == signature;
        if (!isValidRequest)
        {
            return BadRequest();

        }
        //decode the data and save result to database
        string decodedJSON = Encoding.UTF8.GetString(Convert.FromBase64String(data));

        PaymentInfo? paymentInfo = JsonConvert.DeserializeObject<PaymentInfo>(decodedJSON);
        ArgumentNullException.ThrowIfNull(paymentInfo);
        ArgumentNullException.ThrowIfNull(paymentInfo.OrderId);
        Guid correlationId = Guid.Parse(paymentInfo.OrderId);
        await _paymentRepository.UpdateById(correlationId, paymentInfo.Status);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        Payment? payment = await _paymentRepository.GetByCorrelationId(correlationId) ?? throw new KeyNotFoundException($"Payment with id {correlationId} not found");
        await _publishEndpoint.Publish(new FiatToCryptoResponseMessage()
        {
            CorrelationId = correlationId,
            Crypto = payment.Crypto,
            Fiat = payment.Fiat,
            Amount = payment.Amount,
            WalletAddress = payment.WalletAddress
        }, cancellationToken);
        return Ok("Callback processed successfully");
    }

}