using Core.CryptoExchangeRate.Application.Framework.Behavior;
using Core.CryptoExchangeRate.Application.Shared.Contract;
using Core.CryptoExchangeRate.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

        
        return services;
    }
    
 
}
