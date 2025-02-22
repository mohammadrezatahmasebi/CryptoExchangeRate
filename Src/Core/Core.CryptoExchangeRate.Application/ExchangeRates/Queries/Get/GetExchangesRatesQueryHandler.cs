using System.Net;
using Core.CryptoExchangeRate.Application.ExchangeRates.Contracts;
using Core.CryptoExchangeRate.Application.Framework.Queries;
using Core.CryptoExchangeRate.Domain.Framework;

namespace Core.CryptoExchangeRate.Application.ExchangeRates.Queries.Get;

public sealed class GetExchangesRatesQueryHandler(IExchangeRateService exchangeRateService) :
    IQueryHandler<GetExchangesRatesQuery, GetExchangesRatesQueryRes>
{
    public async Task<Result<GetExchangesRatesQueryRes>> Handle(GetExchangesRatesQuery request,
        CancellationToken cancellationToken)
    {
        var response = await exchangeRateService.InquiryCryptoPriceAsync([request.Symbol], cancellationToken);

        return response.HasError
            ? Result.Failure<GetExchangesRatesQueryRes>(new Error(HttpStatusCode.BadGateway,
                response.ValidationError.Message))
            : Result.Success(new GetExchangesRatesQueryRes()
            {
                Rates = response.Rates,
                Symbol = response.BaseCurrency
            });
    }
}