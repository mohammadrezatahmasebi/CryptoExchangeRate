using System.Diagnostics;
using Serilog;

namespace Infra.ExchangeRatesApi.Base;

public static class ServiceCallExtension

{
    public static async Task<CallApiResponseContext<TResponseEntity>> SendAsync<TResponseEntity>(
        this HttpClient client, CallApiRequestContext context, CancellationToken cancellationToken = default)
        where TResponseEntity : notnull
    {
        // 1. Create response 
        var response = new CallApiResponseContext<TResponseEntity>();
        long durationInMilliseconds = 0;
        // 3.Create httpRequestMessage
        using var httpRequestMessage = client.GetHttpRequestMessage(context);

        try
        {
            //var curl = httpRequestMessage.ConvertHttpRequestMessageToCurlCommand();
            // 4.Send request
            Stopwatch stopwatch = Stopwatch.StartNew();
            using var httpResponseMessage = await client.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
            stopwatch.Stop();
            durationInMilliseconds = stopwatch.ElapsedMilliseconds;

            response.HttpResponseMessage = await httpResponseMessage?.Content?.ReadAsStringAsync(cancellationToken);
            response.ResponseHeader =
                httpResponseMessage?.Headers?.ToDictionary(r => r.Key, r => r.Value.FirstOrDefault());
            response.StatusCode = httpResponseMessage.StatusCode;
            response.IsSuccessStatusCode = httpResponseMessage?.IsSuccessStatusCode ?? false;
            response.RequestUri = httpRequestMessage.RequestUri?.ToString();

            // ** Get RequestContent
            if (httpRequestMessage.Content != null)
                response.RequestContent = await httpRequestMessage.Content?.ReadAsStringAsync(cancellationToken);

            // 5.DeserializeObject
            if (response.IsSuccessStatusCode)
            {
                response.Response = response.HttpResponseMessage.ToObject<TResponseEntity>();
                response.IsSuccessDeserializeObject = true;
            }
        }
        // UnKnown or undefined exception
        catch (Exception exp)
        {
            var failur = new ValidationFailure("UnKnown",
                $"ServiceCall has Exception . response: {response.ToJson()} exception {exp.Message}.", -1);
            response.ValidationFailures.Add(failur);
            response.Exception = exp;
        }
        finally
        {
            context.RequestContent?.Dispose();

            LogApiCall(new ApiCallLogModel(
             DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            httpRequestMessage?.Method?.ToString()??"",
            httpRequestMessage?.RequestUri?.ToString()??"",
            (int)response.StatusCode,
            (int)durationInMilliseconds,
            response.IsSuccessStatusCode ? "None" : response?.Exception?.Message,
            "null",  // Assuming user ID is null for now
            httpRequestMessage?.Content?.ToJson(),
            response?.Response?.ToJson(),
            context.TrackCode));
        }
            
        // 7.Return response
        return response;
    }

    public static HttpRequestMessage GetHttpRequestMessage(this HttpClient client, CallApiRequestContext context)
    {
        HttpRequestMessage httpRequestMessage = null;
        try
        {
            httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = context.MethodType;
            if (client.BaseAddress != null)
                httpRequestMessage.RequestUri = new Uri(client.BaseAddress, context.ServiceUrl);
            else
                httpRequestMessage.RequestUri = new Uri(context.ServiceUrl);

            if (context.RequestContent != null)
                httpRequestMessage.Content = context.RequestContent;

            if (context.Headers != null)
                foreach (var header in context.Headers)
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
            return httpRequestMessage;
        }
        catch
        {
            httpRequestMessage?.Dispose();
            throw;
        }
    }

    public static string ConvertHttpRequestMessageToCurlCommand(this HttpRequestMessage request)
    {
        // Initialize the cURL command
        var curlCommand = "curl";

        // Get the request method
        var method = request.Method.ToString().ToLower();
        curlCommand += $" -X {method}";

        // Add the request headers
        foreach (var header in request.Headers)
        {
            curlCommand += $" -H '{header.Key}: {string.Join(", ", header.Value)}'";
        }

        // Add the request content
        if (request.Content != null)
        {
            //var content = JsonConvert.SerializeObject(request.Content.ReadAsAsync<object>().Result);
            var content = request.Content.ToJson();
            curlCommand += $" -d '{content}'";
        }

        // Add the request URI
        curlCommand += $" '{request.RequestUri}'";

        return curlCommand;
    }


    public static async Task<CallApiResponseContext> SendAsync(this HttpClient client,
        CallApiRequestContext context, CancellationToken cancellationToken = default)
    {
        // 1. Create response 
        var response = new CallApiResponseContext();

        // 3.Create httpRequestMessage
        using var httpRequestMessage = new HttpRequestMessage();
        httpRequestMessage.Method = context.MethodType;
        if (client.BaseAddress != null)
            httpRequestMessage.RequestUri = new Uri(client.BaseAddress, context.ServiceUrl);
        else
            httpRequestMessage.RequestUri = new Uri(context.ServiceUrl);

        if (context.RequestContent != null)
            httpRequestMessage.Content = context.RequestContent;

        if (context.Headers != null)
            foreach (var header in context.Headers)
                httpRequestMessage.Headers.Add(header.Key, header.Value);

        try
        {
            // 4.Send request
            using var httpResponseMessage = await client.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
            response.HttpResponseMessage = await httpResponseMessage?.Content?.ReadAsStringAsync(cancellationToken);
            response.ResponseHeader =
                httpResponseMessage?.Headers?.ToDictionary(r => r.Key, r => r.Value.FirstOrDefault());
            response.StatusCode = httpResponseMessage.StatusCode;
            response.IsSuccessStatusCode = httpResponseMessage?.IsSuccessStatusCode ?? false;
            response.RequestUri = httpRequestMessage.RequestUri?.ToString();

            // ** Get RequestContent
            if (httpRequestMessage.Content != null)
                response.RequestContent = await httpRequestMessage.Content?.ReadAsStringAsync(cancellationToken);
        }
        // UnKnown or undefined exception
        catch (Exception exp)
        {
            var failur = new ValidationFailure("UnKnown",
                $"ServiceCall has Exception . response: {response.ToJson()} exception {exp.Message}.", -1);
            response.ValidationFailures.Add(failur);
            response.Exception = exp;
        }
        finally
        {
            context.RequestContent?.Dispose();
        }

        // 7.Return response
        return response;
    }

    static void LogApiCall(ApiCallLogModel logModel)
    {
        Log.Information("{@LogData}", new
        {
            Timestamp = logModel.Timestamp,
            Method = logModel.Method,
            Endpoint = logModel.Endpoint,
            StatusCode = logModel.StatusCode,
            Duration = logModel.Duration,
            Error = logModel.Error,
            UserId = logModel.UserId,
            RequestBody = logModel.RequestBody,
            Resopnsebody = logModel.ResponseBody,
            TrackCode=logModel.TrackCode
        });
    }

}