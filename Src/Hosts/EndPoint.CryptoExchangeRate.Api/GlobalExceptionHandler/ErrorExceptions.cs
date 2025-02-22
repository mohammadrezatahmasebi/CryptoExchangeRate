using System.Net;

namespace EndPoint.CryptoExchangeRate.Api.GlobalExceptionHandler;

public class ErrorExceptions : Exception
{
    public string ApplicationId { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; }

    public new string Message { get; set; }
}