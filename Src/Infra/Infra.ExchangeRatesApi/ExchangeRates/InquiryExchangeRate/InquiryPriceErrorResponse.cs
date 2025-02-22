using System.Text.Json.Serialization;
using Core.CryptoExchangeRate.Application.Shared;

namespace Infra.ExchangeRatesApi.ExchangeRates.InquiryExchangeRate;

public class ExchangeRateErrorResponse : IApiErrorMapper
{
    [JsonPropertyName("error")]
    public ExchangeRateErrorDto  Error{ get; set; }

    public string ErrorMessageGetter => $"{Error.Code},{Error.Info}";
}
public class ExchangeRateErrorDto 
{
    public int Code{ get; set; }

    public string Info { get; set; }
}