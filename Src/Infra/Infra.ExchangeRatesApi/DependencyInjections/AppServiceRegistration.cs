using Core.CryptoExchangeRate.Application.ExchangeRates.Contracts;
using Core.CryptoExchangeRate.Application.Shared;
using Core.CryptoExchangeRate.Application.Shared.Configs;
using Infra.ExchangeRatesApi.Base;
using Infra.ExchangeRatesApi.ExchangeRates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.ExchangeRatesApi.DependencyInjections;

public static class AppServiceRegistration
{
    public static IServiceCollection AddExchangeRateApi(this IServiceCollection services, IConfiguration configuration)
    {
        var option = configuration.GetSection(nameof(AppConfig)).Get<AppConfig>();

        if (option is null)
            throw new ArgumentNullException(nameof(AppConfig));

        services.AddScoped(typeof(IApiService<>), typeof(ApiService<>));
        AddHttpClientConfig(services, option.ExchangeRateConfig, nameof(option.ExchangeRateConfig));
        services.AddScoped<IExchangeRateService, ExchangeRateService>();

        return services;
    }

    private static void AddHttpClientConfig(IServiceCollection services, ApiConfig config, string configName)
    {
        var httpClient = services.AddHttpClient(configName, client =>
        {
            client.BaseAddress = new Uri(config.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(config.Timeout);
        });

        httpClient.SetHandlerLifetime(TimeSpan.FromMinutes(10d))
            .AddCustomPolly();
    }
}