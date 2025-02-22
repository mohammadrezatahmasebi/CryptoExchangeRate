using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Si24.Core.FormBuilder.Domain.Framework;
using Si24.Infra.FormBuilder.Command.Repositories;

namespace Si24.Infra.FormBuilder.Command.Shared.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection CommandConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString =
            configuration.GetConnectionString("WorkflowDbContext") ??
            throw new ArgumentNullException("Connection String not found. check the name of connection string in configuration file");

        services.AddDbContext<FormBuilderCommandContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        
        services.AddScoped<IStepCommandRepository, StepCommandRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
