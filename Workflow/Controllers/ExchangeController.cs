using Microsoft.AspNetCore.Mvc;
using SharedContracts;
using Workflow.Services;
using SharedContracts.Constants;

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

            Guid paymentId = Guid.NewGuid();
            Guid correlationId = Guid.NewGuid();
            await _workflowService.StartFiatOnRampWorkflowAsync(new FiatOnRampMessage
            {
                Fiat = deposit.Currency,
                Crypto = deposit.Crypto,
                Amount = deposit.Amount,
                Currency = deposit.Currency,
                Description = deposit.Description,
                Phone = deposit.Phone,
                CreatedAt = DateTime.Now,
                PaymentId = paymentId,
                CorrelationId = correlationId,
                OrderId = correlationId,
                WalletAddress = deposit.WalletAddress

            });
            var response = new
            {
                PaymentId = paymentId.ToString(),
                Status = "pending"
            };
            return Ok(response);
        }
    }
}
