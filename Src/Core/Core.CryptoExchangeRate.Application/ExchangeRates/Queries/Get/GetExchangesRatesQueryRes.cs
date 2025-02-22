using Core.CryptoExchangeRate.Application.Shared;

namespace Core.CryptoExchangeRate.Application.ExchangeRates.Queries.Get;

public sealed class GetExchangesRatesQueryRes:ServiceResContextBase
{
    public string Symbol { get; set; }
    public Dictionary<string,string> Rates { get; set; }
}