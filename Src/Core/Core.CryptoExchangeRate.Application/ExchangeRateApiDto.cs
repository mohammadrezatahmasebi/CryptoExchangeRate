using Core.CryptoExchangeRate.Application.Shared;

namespace Core.CryptoExchangeRate.Application;

public class ExchangeRateApiDto
    : ServiceResContextBase
{
    public ExchangeRateApiDto(string baseCurrency, string date, Dictionary<string, decimal> rates)
    {
        BaseCurrency = baseCurrency;
        Date = date;
        Rates = rates;
    }

    public string BaseCurrency { get; set; }
    public string Date { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
}