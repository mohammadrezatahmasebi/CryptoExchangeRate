using Core.CryptoExchangeRate.Application.Shared.Dependency;
using EndPoint.CryptoExchangeRate.Api.Extensions.Dependency;
using EndPoint.CryptoExchangeRate.Api.GlobalExceptionHandler;
using Infra.ExchangeRatesApi.DependencyInjections;

namespace EndPoint.CryptoExchangeRate.Api.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureService(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddHealthChecks();
        builder.Services.AddExchangeRateApi(builder.Configuration);
        builder.Services.ApplicationConfigureServices(builder.Configuration);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMemoryCache();
        builder.Services.EndPointConfigureServices();


        try
        {
            var a= builder.Build();



            return a;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<GlobalException>();

        app.MapControllers();

        app.UseHealthChecks("/health");

        return app;
    }
}