using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Si24.Infra.FormBuilder.Query.Configs.Contexts;
using Si24.Infra.FormBuilder.Query.Repositories;

namespace Si24.Infra.FormBuilder.Query.DependencyLoaders;


public static class ServiceRegisterer
{

    public static IServiceCollection QueryConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("HealthProfileDbContext") ??
            throw new ArgumentNullException("Connection String not found. check the name of connection string in configuration file");


        services.AddDbContext<FormBuilderQueryContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        
        services.AddScoped<IStepQueryRepository, StepQueryRepository>();
     
        return services;
    }
}
