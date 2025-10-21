using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Exchange(string currencyPair)
        {
            await _workflowService.StartFiatToCryptoWorkflowAsync(new FiatToCryptoMessage
            {
                Fiat = currencyPair.Split('-')[0],
                Crypto = currencyPair.Split('-')[1],
                Amount = 100 // Example amount
            });
            return Ok($"Exchanging currency pair: {currencyPair}");
        }
    }
}
