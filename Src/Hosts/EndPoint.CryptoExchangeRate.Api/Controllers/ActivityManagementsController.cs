using Core.CryptoExchangeRate.Application.ExchangeRates.Queries.Get;
using EndPoint.CryptoExchangeRate.Api.Framework;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.CryptoExchangeRate.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRatesController : BaseController
    {
        private readonly IMediator _mediator;

        public ExchangeRatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRates([FromQuery] string symbol)
        {
            var result = await _mediator.Send(new GetExchangesRatesQuery()
            {
                Symbol = symbol
            });

            return EXResult(result);
        }
    }
}