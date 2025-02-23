namespace EndPoint.CryptoExchangeRate.Api.GlobalExceptions;

public interface IGlobaException
{
    Task InvokeAsync(HttpContext context);
}