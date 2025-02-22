namespace Core.CryptoExchangeRate.Application.Shared.Models;

public sealed class ErrorResponse
{
    public int StatusCode { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}