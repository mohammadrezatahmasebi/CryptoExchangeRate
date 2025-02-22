using Core.CryptoExchangeRate.Application.ExchangeRates.Contracts;
using Core.CryptoExchangeRate.Application.Framework.Queries;
using Core.CryptoExchangeRate.Domain.Framework;

namespace Core.CryptoExchangeRate.Application.ExchangeRates.Queries.Get;

public sealed class GetExchangesRatesQueryHandler(IExchangeRateService exchangeRateService) : 
    IQueryHandler<GetExchangesRatesQuery,GetExchangesRatesQueryRes>
{
    
    public async Task<Result<GetExchangesRatesQueryRes>> Handle(GetExchangesRatesQuery request, CancellationToken cancellationToken)
    {
        var response = await exchangeRateService.InquiryCryptoPriceAsync([request.Symbol],cancellationToken);

        if (response.HasError)
            return Result.Failure<GetExchangesRatesQueryRes>(Error.NullValue);


        return Result.Success(new GetExchangesRatesQueryRes());
    }
}