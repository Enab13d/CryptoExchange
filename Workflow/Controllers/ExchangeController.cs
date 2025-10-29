using Microsoft.AspNetCore.Mvc;
using SharedContracts;
using Workflow.Services;

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

        [HttpPost("{currencyPair}")]
        public async Task<IActionResult> Exchange(string currencyPair, DepositDTO deposit)
        {
            await _workflowService.StartFiatToCryptoWorkflowAsync(new FiatToCryptoMessage
            {
                Fiat = currencyPair.Split('-')[0],
                Crypto = currencyPair.Split('-')[1],
                Amount = deposit.Amount,
                Currency = deposit.Currency,
                Description = deposit.Description,
                Phone = deposit.Phone,
                Card = deposit.Card,
                CardExpirationMonth = deposit.CardExpirationMonth,
                CardExpirationYear = deposit.CardExpirationYear,
                CardCVV = deposit.CardCVV


            });
            return Ok($"Exchanging currency pair: {currencyPair}");
        }
    }
}
