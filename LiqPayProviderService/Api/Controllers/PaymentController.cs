

using System.Text;
using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Constants;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedContracts;

namespace LiqPayProviderService.Api.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class PaymentController(IWebhookService webhookService, IPaymentRepository paymentRepository, IUnitOfWork unitOfWork, ILiqpayService liqpayService, ILogger<PaymentController> logger) : ControllerBase
{

    private readonly ILiqpayService _liqpayService = liqpayService;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IWebhookService _webhookService = webhookService;
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
        _paymentRepository.UpdateById(correlationId, paymentInfo.Status);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _webhookService.Publish(correlationId, DepositStatus.Success, cancellationToken);
        return Ok("Callback processed successfully");
    }
    [HttpPost("deposit")]
    public async Task<IActionResult> HandleDeposit(DepositDTO deposit, CancellationToken cancellationToken)
    {
        PaymentDataDTO paymentData = _liqpayService.PreparePaymentData(deposit);
        return Ok(paymentData);
    }
}