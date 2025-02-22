namespace Core.CryptoExchangeRate.Application.ExchangeRates.Queries.Get;

public class GetExchangesRatesQueryRes 
{
    public string Symbol { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
}