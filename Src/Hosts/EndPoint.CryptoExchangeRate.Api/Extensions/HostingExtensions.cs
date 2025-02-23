using System.Threading.RateLimiting;
using Core.CryptoExchangeRate.Application.Shared.Dependency;
using EndPoint.CryptoExchangeRate.Api.Extensions.Dependency;
using EndPoint.CryptoExchangeRate.Api.GlobalExceptions;
using Infra.CryptoExchangeRate.Db.Dependency;
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
        builder.Services.DbConfigureServices();

        AddCustomRateLimiter(builder);

        AddCustomToken(builder);

        builder.Services.AddAuthorization();

        return builder.Build();
    }

    private static void AddCustomToken(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["Jwt:Issuer"];
                options.Audience = builder.Configuration["Jwt:Audience"];
                options.RequireHttpsMetadata = false;
            });
    }

    private static void AddCustomRateLimiter(WebApplicationBuilder builder)
    {
        var rateLimitConfig = builder.Configuration.GetSection(RateLimitConfig.SectionName).Get<RateLimitConfig>();

        builder.Services.AddRateLimiter(options =>
        {
            options.AddPolicy("fixed-by-ip", httpContext =>
            {
                var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = rateLimitConfig?.PermitLimit ?? 10, // Default to 10 if null
                    Window = TimeSpan.FromSeconds(rateLimitConfig?.WindowSeconds ?? 30),
                    QueueLimit = rateLimitConfig?.QueueLimit ?? 0
                });
            });
        });
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoExchangeRate API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<GlobalException>();

        app.MapControllers();

        app.UseHealthChecks("/health");

        return app;
    }
}

public class RateLimitConfig
{
    public const string SectionName = "RateLimiting";
    public int PermitLimit { get; set; }
    public int WindowSeconds { get; set; }
    public int QueueLimit { get; set; }
}