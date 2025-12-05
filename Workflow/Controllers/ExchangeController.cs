using Microsoft.AspNetCore.Mvc;
using SharedContracts;
using Workflow.Services;
using Microsoft.AspNetCore.Authorization;
using Workflow.Extensions;
using System.Reflection;

namespace Workflow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdministratorRole")]
    public class ExchangeController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;
        private readonly ILogger<ExchangeController> _logger;

        public ExchangeController(IWorkflowService workflowService, ILogger<ExchangeController> logger)
        {
            _workflowService = workflowService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Exchange(DepositDTO deposit)
        {

            Guid paymentId = Guid.NewGuid();
            Guid correlationId = Guid.NewGuid();
            User user = HttpContext.UserFromClaims();
            Type type = user.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (var header in Request.Headers)
            {
                _logger.LogInformation("{Header}: {Value}", header.Key, string.Join(",", header.Value.ToString()));
            }
            foreach (PropertyInfo property in properties)
            {
                _logger.LogInformation("{PropertyName}:{PropertyValue}", property.Name, property.GetValue(user));

            }

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
                WalletAddress = deposit.WalletAddress,
                User = user

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
