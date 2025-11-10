using Microsoft.AspNetCore.Mvc;
using SharedContracts;
using Workflow.Services;
using Workflow.Domain.Constants;

namespace Workflow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;

        public ExchangeController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost]
        public async Task<IActionResult> Exchange(DepositDTO deposit)
        {
            bool isCurrencyValid = Enum.TryParse(deposit.Currency, false, out Currency currency);
            if (!isCurrencyValid)
            {
                return BadRequest($"Currency {deposit.Currency} is not supported");

            }
            bool isCryptoValid = Enum.TryParse(deposit.Crypto, false, out Crypto crypto);
            if (!isCryptoValid)
            {
                return BadRequest($"Crypto {deposit.Crypto} is not supported");
            }

            Guid paymentId = Guid.NewGuid();
            await _workflowService.StartFiatToCryptoWorkflowAsync(new FiatToCryptoMessage
            {
                Fiat = deposit.Currency,
                Crypto = deposit.Crypto,
                Amount = deposit.Amount,
                Currency = deposit.Currency,
                Description = deposit.Description,
                Phone = deposit.Phone,
                CreatedAt = DateTime.Now,
                PaymentId = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid()


            });
            var response = new
            {
                PaymentId = paymentId,
                Status = "pending"
            };
            return Ok(response);
        }
    }
}
