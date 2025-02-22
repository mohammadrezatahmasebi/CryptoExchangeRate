using EndPoint.CryptoExchangeRate.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var app = builder.ConfigureService();

// Configure the HTTP request pipeline.
app.ConfigurePipeline();

app.Run();