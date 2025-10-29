

using System.Text;
using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Constants;
using LiqPayProviderService.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LiqPayProviderService.Api.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class PaymentController(ILiqpayClient liqpayClient, IWebhookService webhookService, IPaymentRepository paymentRepository) : ControllerBase
{
    private readonly ILiqpayClient _liqpayClient = liqpayClient;

    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IWebhookService _webhookService = webhookService;
    [HttpPost("callback")]
    public async Task<IActionResult> HandleLiqpayCallback(CancellationToken cancellationToken)
    {
        IFormCollection form = await Request.ReadFormAsync(cancellationToken);

        var data = form["data"].ToString();
        var signature = form["signature"].ToString();
        bool isValidRequest = _liqpayClient.CreateSignature(data) == signature;
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
        _paymentRepository.UpdateById(correlationId, PaymentStatus.Success);
        await _paymentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        await _webhookService.Publish(correlationId, cancellationToken);
        return Ok("Callback processed successfully");
    }
}