using System.Text.Json.Serialization;
using Core.CryptoExchangeRate.Application.Shared;

namespace Infra.ExchangeRatesApi.ExchangeRates.InquiryExchangeRate;

public sealed class ExchangeRateApiResponse:ServiceResContextBase
{
    public bool Success { get; set; }
    public int Timestamp { get; set; }
    [JsonPropertyName("base")]
    public string BaseCurrency { get; set; }
    public string Date { get; set; }
    public Dictionary<string,decimal> Rates { get; set; }
}