
using Core.CryptoExchangeRate.Application.Shared.Models;

namespace Core.CryptoExchangeRate.Application.Shared.Configs;

public class AppConfig
{
    public ExchangeRateConfig ExchangeRateConfig { get; set; }
}

public sealed class ExchangeRateConfig : ApiConfig
{
    public string InquiryCryptoPriceUrl { get; set; }
}



public class ApiConfig
{
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; }
    public Dictionary<string, string> ErrorCode { get; set; } = [];
    public int Timeout { get; set; }
    public List<ErrorMapping> ErrorMappings { get; set; } = [];
}