using System.Net;
using System.Text.Json.Serialization;

namespace Infra.ExchangeRatesApi.Base;

public readonly struct CallApiRequestContext(
    HttpMethod httpMethod = null,
    string serviceUrl = null,
    HttpContent httpContent = null,
    IDictionary<string, string> header = null,
    string trackCode=null)
{
    [JsonIgnore] public HttpContent RequestContent { get; init; } = httpContent;

    public HttpMethod MethodType { get; init; } = httpMethod ?? HttpMethod.Get;

    public string ServiceUrl { get; init; } = serviceUrl;

    [JsonIgnore] public IDictionary<string, string> Headers { get; init; } = header;
    [JsonIgnore] public string? TrackCode { get; init; } = trackCode;

}

public class CallApiResponseContext
{
    public string HttpResponseMessage { get; set; }

    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.RequestTimeout;

    public bool IsSuccessStatusCode { get; set; } = false;

    public bool IsSuccessDeserializeObject { get; set; } = false;

    public List<ValidationFailure> ValidationFailures { get; set; } = new();

    public string RequestUri { get; set; }

    [JsonIgnore] public Exception Exception { get; set; }

    public string RequestContent { get; set; }

    public Dictionary<string, string> ResponseHeader { get; set; } = new();
}

//TODO struct
public sealed class CallApiResponseContext<TResponse> : CallApiResponseContext where TResponse : notnull
{
    public TResponse Response { get; set; }
}

public readonly struct ValidationFailure(string propertyName, string errorMessage, int errorCode)
{
    public string PropertyName { get; init; } = propertyName;

    public string ErrorMessage { get; init; } = errorMessage;

    public int ErrorCode { get; init; } = errorCode;
}