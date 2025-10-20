using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Workflow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        [HttpPost("{currencyPair}")]
        public IActionResult Exchange(string currencyPair)
        {
            return Ok($"Exchanging currency pair: {currencyPair}");
        }
    }
}
