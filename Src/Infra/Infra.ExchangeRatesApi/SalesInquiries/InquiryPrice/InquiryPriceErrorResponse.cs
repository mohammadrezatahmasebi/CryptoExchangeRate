using System.Text.Json.Serialization;
using Core.CryptoExchangeRate.Application.Shared;

namespace Infra.ExchangeRatesApi.SalesInquiries.InquiryPrice;

public class SalesInquiryErrorResponse : IApiErrorMapper
{
    [JsonPropertyName("")]
    public List<string>  Errors{ get; set; }

    public string ErrorMessageGetter => string.Join(",", Errors);
}