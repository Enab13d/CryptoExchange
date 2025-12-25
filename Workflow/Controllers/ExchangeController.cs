using Microsoft.AspNetCore.Mvc;
using SharedContracts;
using Workflow.Services;
using Microsoft.AspNetCore.Authorization;
using Workflow.Extensions;
using Workflow.Infrastructure.Policies;

namespace Workflow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = WorkflowAuthorizationPolicy.UserPolicy)]
    public class ExchangeController(IWorkflowService workflowService) : ControllerBase
    {
        private readonly IWorkflowService _workflowService = workflowService;

        [HttpPost]
        public async Task<IActionResult> Exchange(DepositDTO deposit)
        {

            Guid paymentId = Guid.NewGuid();
            Guid correlationId = Guid.NewGuid();
            User user = HttpContext.UserFromClaims();

            await _workflowService.StartFiatOnRampWorkflowAsync(new FiatOnRampRequested
            {
                Fiat = deposit.Currency,
                Crypto = deposit.Crypto,
                Amount = deposit.Amount,
                Description = deposit.Description,
                Phone = deposit.Phone,
                CreatedAt = DateTime.Now,
                PaymentId = paymentId,
                CorrelationId = correlationId,
                OrderId = correlationId,
                WalletAddress = deposit.WalletAddress,
                UserId = user.UserId

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
