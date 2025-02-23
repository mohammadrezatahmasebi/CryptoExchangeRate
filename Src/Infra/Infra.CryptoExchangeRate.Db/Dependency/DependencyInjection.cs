using Core.CryptoExchangeRate.Application.AuthServices.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CryptoExchangeRate.Db.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection DbConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, UserRepository>();
        
        return services;
    }
    
 
}
