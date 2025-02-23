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
        var eurdRates = await exchangeRateService.InquiryCryptoPriceAsync([
            ExchangeRatesConst.EUR,
            ExchangeRatesConst.AUD,
            ExchangeRatesConst.BRL,
            ExchangeRatesConst.GBP
        ], cancellationToken);


        if (eurdRates.HasError)
            return Result.Failure<GetExchangesRatesQueryRes>(new Error(HttpStatusCode.BadGateway,
                eurdRates.ValidationError.Message));

        var cryptoResponse = await exchangeRateService.InquiryCryptoPriceAsync([request.Symbol], cancellationToken);

        if (cryptoResponse.HasError)
            return Result.Failure<GetExchangesRatesQueryRes>(new Error(HttpStatusCode.BadGateway,
                cryptoResponse.ValidationError.Message));


        var rates = new Dictionary<string, decimal>
        {
            { ExchangeRatesConst.EUR, eurdRates.Rates[ExchangeRatesConst.EUR] },
            { ExchangeRatesConst.AUD, cryptoResponse.Rates[request.Symbol] * eurdRates.Rates[ExchangeRatesConst.AUD] },
            { ExchangeRatesConst.BRL, cryptoResponse.Rates[request.Symbol] * eurdRates.Rates[ExchangeRatesConst.BRL] },
            { ExchangeRatesConst.GBP, cryptoResponse.Rates[request.Symbol] * eurdRates.Rates[ExchangeRatesConst.GBP] },
        };

        return Result.Success(new GetExchangesRatesQueryRes()
        {
            Rates = rates,
            Symbol = request.Symbol
        });
    }
}