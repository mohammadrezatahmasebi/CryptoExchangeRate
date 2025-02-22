using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Si24.Infra.FormBuilder.Command.DependencyLoaders;


public static class ServiceRegisterer
{

    public static IServiceCollection AddInfraCommandRepository(this IServiceCollection services, IConfiguration configuration)
    {
        

     
        return services;
    }
}
