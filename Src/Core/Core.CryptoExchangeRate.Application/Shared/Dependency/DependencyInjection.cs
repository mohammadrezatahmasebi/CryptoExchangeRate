using Core.CryptoExchangeRate.Application.Framework.Behavior;
using Core.CryptoExchangeRate.Application.Shared.Contract;
using Core.CryptoExchangeRate.Domain.Framework;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CryptoExchangeRate.Application.Shared.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection ApplicationConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var application = typeof(IAssemblyMarker);

        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssembly(application.Assembly);
        });


        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        
        return services;
    }
    
 
}
