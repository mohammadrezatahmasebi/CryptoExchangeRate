using Core.CryptoExchangeRate.Application;
using Core.CryptoExchangeRate.Application.ExchangeRates.Contracts;
using Core.CryptoExchangeRate.Application.Shared;
using Core.CryptoExchangeRate.Application.Shared.Configs;
using Core.CryptoExchangeRate.Application.Shared.Models;
using Infra.ExchangeRatesApi.ExchangeRates.Consts;
using Infra.ExchangeRatesApi.ExchangeRates.InquiryExchangeRate;
using Microsoft.Extensions.Options;

namespace Infra.ExchangeRatesApi.ExchangeRates;

public sealed record ExchangeRateService(
    IOptions<AppConfig> Options,
    IApiService<ExchangeRateService> ApiService) : IExchangeRateService
{
    private readonly ExchangeRateConfig _exchangeRateConfig = Options.Value.ExchangeRateConfig;

    public async Task<ExchangeRateApiDto> InquiryCryptoPriceAsync(string[] symbol,
        CancellationToken cancellationToken = new())
    {
        var apiParameter = new ApiParameter
        {
            ApiConfig = _exchangeRateConfig,
            HttpMethod = HttpMethod.Get,
            ApiUrl = _exchangeRateConfig.InquiryCryptoPriceUrl,
            CheckErrorInSuccessStatus = true,
            HttpHeader = new Dictionary<string, string> { { ExchangeRateConst.APIKEY, _exchangeRateConfig.ApiKey } },
            QueryParams = new Dictionary<string, string[]>()
            {
                { ExchangeRateConst.SYMBOL, symbol }
            }
        };

        var response =
            await ApiService.CallApi<ExchangeRateApiResponse, ExchangeRateErrorResponse>(apiParameter,
                cancellationToken: cancellationToken);


        return new ExchangeRateApiDto(response.BaseCurrency, response.Date, response.Rates)
        {
            ValidationError = response.ValidationError
        };
    }
}