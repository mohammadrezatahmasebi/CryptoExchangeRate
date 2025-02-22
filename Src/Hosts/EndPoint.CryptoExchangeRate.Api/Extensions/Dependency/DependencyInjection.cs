namespace EndPoint.CryptoExchangeRate.Api.Extensions.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection EndPointConfigureServices(this IServiceCollection services)
    {
        services.AddSwaggerGen();


        return services;
    }
}