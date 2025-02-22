using Core.CryptoExchangeRate.Application.Shared;
using Infra.ExchangeRatesApi.Base;
using Infra.ExchangeRatesApi.SalesInquiries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.ExchangeRatesApi.DependencyInjections;

public static class AppServiceRegistration
{

    
    public static IServiceCollection AddInfrastructures(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MapLetterSetting>(configuration.GetSection(MapLetterSetting.Section));

        services.AddStackExchangeRedisCache(options =>
        {
            var redisConfig = configuration.GetSection("Redis");
            options.Configuration = redisConfig["ConnectionString"];
        });
        
        services.AddScoped(typeof(IApiService<>), typeof(ApiService<>));

        return services;
    }
}