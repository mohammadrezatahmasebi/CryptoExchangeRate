namespace Core.CryptoExchangeRate.Application.ExchangeRates.Contracts;

public interface IExchangeRateService
{
    Task<ExchangeRateApiDto> InquiryCryptoPriceAsync(string[] symbol,
        CancellationToken cancellationToken = new());
}