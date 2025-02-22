using System.Net;

namespace Core.CryptoExchangeRate.Domain.Framework;

public record Error(HttpStatusCode Code, string Message)
{
    public static Error None = new(HttpStatusCode.Gone, string.Empty);

    public static Error NullValue = new(HttpStatusCode.NotFound, "Null value was provided");
}
