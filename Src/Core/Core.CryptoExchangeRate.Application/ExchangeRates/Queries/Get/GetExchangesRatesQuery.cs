using Core.CryptoExchangeRate.Application.Framework.Queries;

namespace Core.CryptoExchangeRate.Application.ExchangeRates.Queries.Get;

public class GetExchangesRatesQuery:IQuery<GetExchangesRatesQueryRes>,ICacheableRequest<GetExchangesRatesQueryRes>
{
    public string CacheKey => $"{Constants.GetExchangesRateCacheKey}:{Symbol}";
    public Func<GetExchangesRatesQueryRes, DateTimeOffset> ConditionExpiration => static _ =>DateTimeOffset.Now.AddMinutes(1);
    public string Symbol { get; init; }
}