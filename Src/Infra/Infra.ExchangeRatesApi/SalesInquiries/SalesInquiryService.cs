using Core.CryptoExchangeRate.Application.Shared;
using Core.CryptoExchangeRate.Application.Shared.Configs;
using Core.CryptoExchangeRate.Application.Shared.Models;
using Infra.ExchangeRatesApi.SalesInquiries.InquiryPrice;
using Microsoft.Extensions.Options;

namespace Infra.ExchangeRatesApi.SalesInquiries;

public sealed record SalesInquiryService(
    IOptions<AppConfig> Options,
    IApiService<SalesInquiryService> ApiService) : ISalesInquiryService
{
    private readonly SalesInquiryConfig _salesConfig = Options.Value.SalesInquiryConfig;

    public async Task<InquiryPriceResponse> InquiryByPlaqueAsync(InquiryPriceRequest request,
        CancellationToken cancellationToken = new())
    {
        var apiParameter = new ApiParameter
        {
            ApiConfig = _salesConfig,
            HttpMethod = HttpMethod.Post,
            ApiUrl = _salesConfig.InquiryPrice,
            CheckErrorInSuccessStatus = true,
            HttpContent = request.ToHttpContent(),
            HttpHeader = []
        };
        
        var response =
            await ApiService.CallApi<InquiryPriceResponse, SalesInquiryErrorResponse>(apiParameter,
                cancellationToken:  cancellationToken);


        HandleException(response);

        return response;
    }

    private static void HandleException(ServiceResContextBase response)
    {
        if (response.HasError)
            throw new SalesInquiryException((ushort)response.ValidationError.HttpStatusCode,
                response.ValidationError.Message);
    }
}