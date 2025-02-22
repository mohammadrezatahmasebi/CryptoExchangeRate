namespace Core.CryptoExchangeRate.Application.Shared.Models;

public sealed class ErrorMapping
{
    public ErrorResponse Inbound { get; set; } = new();
    public ErrorResponse Outbound { get; set; } = new();
}