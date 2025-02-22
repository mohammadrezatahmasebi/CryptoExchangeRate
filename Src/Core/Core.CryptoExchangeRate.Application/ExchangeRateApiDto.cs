using Core.CryptoExchangeRate.Application.Shared;

namespace Core.CryptoExchangeRate.Application;

public class  ExchangeRateApiDto(string BaseCurrency, string Date, Dictionary<string, decimal> Rates):ServiceResContextBase
{
    
}