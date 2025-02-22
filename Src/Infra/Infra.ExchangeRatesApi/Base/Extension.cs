using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Infra.ExchangeRatesApi.Base;

public static partial class Extension
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter()
        },
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        //UnknownTypeHandling = 
    };

    public static readonly JsonSerializerOptions JsonSerializerOptionsWithoutEnumString = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        //UnknownTypeHandling = 
    };
#pragma warning disable CA1802
    private static readonly RegexOptions RegexOptions =
        RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase;
#pragma warning restore CA1802


    public static void AddCustomPolly(this IHttpClientBuilder builder, int breaking = 20,
        int durationOfBreakSecond = 30, byte retryCount = 1)
    {
        builder.AddResilienceHandler(nameof(AddRetryPolicy), (pipelineBuilder, _) =>
        {
            pipelineBuilder.AddTimeout(TimeSpan.FromSeconds(30));
            AddCircuitBreakerPolicy(pipelineBuilder, breaking, TimeSpan.FromSeconds(durationOfBreakSecond));
            AddRetryPolicy(pipelineBuilder, retryCount);
        });
    }

    public static void AddRetryPolicy(ResiliencePipelineBuilder<HttpResponseMessage> builder, int retryCount = 1)
        => builder.AddRetry(new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = retryCount,
            Delay = TimeSpan.FromSeconds(1),
            BackoffType = DelayBackoffType.Linear
        });

    public static void AddCircuitBreakerPolicy(ResiliencePipelineBuilder<HttpResponseMessage> builder,
        int beforeBreaking, TimeSpan durationOfBreak)
        => builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
        {
            BreakDuration = durationOfBreak,
            MinimumThroughput = beforeBreaking,
            FailureRatio = 0.7,
            SamplingDuration = TimeSpan.FromSeconds(30),
        });


    public static string ToJson(this object source, bool maskSensitiveData = false)
    {
        return maskSensitiveData
            ? JsonSerializer.Serialize(source, JsonSerializerOptions)
            : JsonSerializer.Serialize(source, JsonSerializerOptions);
    }

    public static byte[] ToJsonUtf8Bytes(this object source)
    {
        return JsonSerializer.SerializeToUtf8Bytes(source, JsonSerializerOptions);
    }

    public static T ToObject<T>(this string source)
    {
        return JsonSerializer.Deserialize<T>((ReadOnlySpan<char>)source, JsonSerializerOptions);
    }

    public static T ToObject<T>(this byte[] source)
    {
        return JsonSerializer.Deserialize<T>((ReadOnlySpan<byte>)source, JsonSerializerOptions);
    }

    public static string ToJson(this object source, JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(source, options);
    }

    public static string ToJsonWithLengthLimit(this object source, int maxlength = 4096,
        JsonSerializerOptions options = null)
    {
        var serialize = JsonSerializer.Serialize(source, options ?? JsonSerializerOptions);
        var length = Math.Min(maxlength, serialize.Length);
        return serialize[..length];
    }

    public static byte[] ToJsonUtf8Bytes(this object source, JsonSerializerOptions options)
    {
        return JsonSerializer.SerializeToUtf8Bytes(source, options);
    }

    public static T ToObject<T>(this string source, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<T>((ReadOnlySpan<char>)source, options);
    }


    public static T ConvertValue<T, TU>(TU value) where TU : IConvertible
    {
        return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
    }




    public static HttpContent ToHttpContent(this object requestModel, Encoding encoding = null,
        string mediaType = "application/json")
    {
        return new StringContent(requestModel.ToJson(), encoding ?? Encoding.UTF8, mediaType);
    }

   


    public static Dictionary<string, string> ToHeader(this (string, string)[] keyValue)
    {
        if (keyValue == null)
        {
            return new();
        }

        var header = keyValue.ToDictionary<string, string>();

        return header;
    }
    

    public static bool Contains(this List<string> source, string value, StringComparison comparision)
        => source?.Any(item => string.Equals(item, value, comparision)) ?? false;

    public static string ToUrlEncode(this string source)
    {
        return WebUtility.UrlEncode(source);
    }

    public static string ToUrlDecode(this string source)
    {
        return WebUtility.UrlDecode(source);
    }


}