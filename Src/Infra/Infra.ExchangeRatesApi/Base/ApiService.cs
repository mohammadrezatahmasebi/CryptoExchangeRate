using System.Net;
using Core.CryptoExchangeRate.Application.Shared;
using Core.CryptoExchangeRate.Application.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infra.ExchangeRatesApi.Base;

public sealed class ApiService<T> : IApiService<T>
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<T> logger;
    private string? token;
    private readonly string _trackCode;


    public ApiService(IHttpClientFactory httpClientFactory,
        ILogger<T> logger, IHttpContextAccessor httpContextAccessor)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;

        //  _trackCode=httpContextAccessor.HttpContext?.Items[ApiServiceConst.TRACKING_CODE]?.ToString()??"";
    }


    public async Task<TRes> CallApi<TRes>(ApiParameter apiParameter, CancellationToken cancellationToken = new())
        where TRes : ServiceResContextBase, new()
    {
        var response = await SendCallApi<TRes>(apiParameter, cancellationToken: cancellationToken);

        return ExceptionHandling<TRes, DefaultErrorMapper>(apiParameter, response.responseContext,
            response.callRequestContext
            , apiParameter.CheckErrorInSuccessStatus, apiParameter.SuccessCode);
    }

    public async Task<TRes> CallApi<TRes, TErrorMapper>(ApiParameter apiParameter,
        CancellationToken cancellationToken = new())
        where TRes : ServiceResContextBase, new()
        where TErrorMapper : IApiErrorMapper
    {
        var responseAoi = await SendCallApi<TRes>(apiParameter, cancellationToken: cancellationToken);

        var finalResponse = ExceptionHandling<TRes, TErrorMapper>(apiParameter, responseAoi.responseContext,
            responseAoi.callRequestContext
            , apiParameter.CheckErrorInSuccessStatus, apiParameter.SuccessCode);

        if (finalResponse.ValidationError.HttpStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            responseAoi = await SendCallApi<TRes>(apiParameter, cancellationToken: cancellationToken);

            finalResponse = ExceptionHandling<TRes, TErrorMapper>(apiParameter, responseAoi.responseContext,
                responseAoi.callRequestContext,
                apiParameter.CheckErrorInSuccessStatus, apiParameter.SuccessCode, chackAturize: false);
        }

        return finalResponse;
    }


    private async Task<(CallApiResponseContext<TRes> responseContext, CallApiRequestContext callRequestContext)>
        SendCallApi<TRes>(ApiParameter apiParameter,
            CancellationToken cancellationToken = new())
        where TRes : ServiceResContextBase, new()
    {
        var clientName = GetClientName(apiParameter);

        var httpClient = httpClientFactory.CreateClient(clientName);

        var url = AddQueryString(apiParameter.ApiUrl, apiParameter.QueryParams);


        var callApiRequestContext = new CallApiRequestContext(apiParameter.HttpMethod, url,
            header: apiParameter.HttpHeader,
            httpContent: apiParameter.HttpContent,
            trackCode: _trackCode);

        var response = await httpClient.SendAsync<TRes>(callApiRequestContext, cancellationToken);

        return (response, callApiRequestContext);
    }

    private string GetClientName(ApiParameter apiParameter)
    {
        var clientName = apiParameter.ApiConfig.GetType().FullName?.Split(".").LastOrDefault();
        return clientName;
    }

    private TRes ExceptionHandling<TRes, TErrorMapper>(ApiParameter apiParameter,
        CallApiResponseContext<TRes> context
        , CallApiRequestContext request, bool checkErrorInSuccessStatus = false, int successCode = 0,
        bool chackAturize = true)
        where TRes : ServiceResContextBase, new()
        where TErrorMapper : IApiErrorMapper
    {
        var errorRes = new CodeMessageRes();

        if (!string.IsNullOrWhiteSpace(context.HttpResponseMessage))
        {
            try
            {
                errorRes = context.HttpResponseMessage.ToObject<CodeMessageRes>();
                if (errorRes.Message == null) errorRes.Message = errorRes.Messages;
            }
            catch
            {
            }
        }

        string? message = GetErrorMessage<TErrorMapper>(errorRes, context.HttpResponseMessage);


        if (string.IsNullOrEmpty(message) && context.IsSuccessStatusCode && context.Exception == null
            && !HasErrorInSuccessfulStatus(checkErrorInSuccessStatus, errorRes,
                successCode))
        {
            return context.Response;
        }

        if (context.StatusCode == HttpStatusCode.NotFound &&
            string.IsNullOrEmpty(context.HttpResponseMessage))
        {
            return new TRes
            {
                ValidationError = new ValidationError(((int)HttpStatusCode.NotFound).ToString(),
                    MessageText.NotFountInThirdPartyApi
                    , httpStatusCode: (int)HttpStatusCode.ExpectationFailed)
            };
        }

        if (context.StatusCode == HttpStatusCode.Unauthorized && chackAturize)
        {
            var unauthorizedError = apiParameter.ApiConfig.ErrorCode.FirstOrDefault(r =>
                r.Key.Equals(nameof(HttpStatusCode.Unauthorized), StringComparison.OrdinalIgnoreCase));

            return new TRes
            {
                ValidationError = new ValidationError(unauthorizedError.Key, unauthorizedError.Value,
                    httpStatusCode: (int)context.StatusCode)
            };
        }

        if (context.StatusCode == HttpStatusCode.RequestTimeout)
        {
            return new TRes
            {
                ValidationError = new ValidationError(((int)HttpStatusCode.RequestTimeout).ToString(),
                    MessageText.RequestTimeoutInThirdPartyApi
                    , httpStatusCode: (int)HttpStatusCode.RequestTimeout)
            };
        }


        var customError = apiParameter.ApiConfig.ErrorMappings.FirstOrDefault(r =>
            r.Inbound.StatusCode == (int)context.StatusCode && (
                string.IsNullOrEmpty(r.Inbound.ErrorCode) &&
                (string.IsNullOrWhiteSpace(r.Inbound.ErrorMessage) ||
                 r.Inbound.ErrorMessage.Equals(errorRes.ErrorMessage?.Trim([' ', '.']))) ||
                !string.IsNullOrEmpty(r.Inbound.ErrorCode) &&
                r.Inbound.ErrorCode.Equals(errorRes.Code.ToString())
            ));

        if (customError != default)
        {
            return new TRes
            {
                ValidationError = new ValidationError(customError.Outbound.ErrorCode, customError.Outbound.ErrorMessage,
                    customError.Outbound.StatusCode)
            };
        }

        if (!string.IsNullOrWhiteSpace(message) || !string.IsNullOrWhiteSpace(errorRes.ErrorMessage))
        {
            return new TRes
            {
                ValidationError = new ValidationError(errorRes.Code.ToString(),
                    string.IsNullOrWhiteSpace(errorRes.ErrorMessage) ? message : errorRes.ErrorMessage)
            };
        }

        var clientName = GetClientName(apiParameter);

        logger.LogError($"{clientName} service error request: {request.ToJson()}, res: {context.ToJson()}");

        var unhandledError = apiParameter.ApiConfig.ErrorCode.FirstOrDefault(r =>
            r.Key.Equals(Constants.UnhandledError, StringComparison.OrdinalIgnoreCase));

        return new TRes
        {
            ValidationError = ValidationError.UnhandledValidationError(Constants.UnhandledError, unhandledError.Value)
        };
    }

    private bool HasErrorInSuccessfulStatus(bool checkErrorInSuccessStatus, CodeMessageRes errorRes, int successCode)
    {
        if (checkErrorInSuccessStatus && errorRes.Code != successCode)
            return true;

        return false;
    }

    private string GetErrorMessage<TErrorMapper>(CodeMessageRes errorRes, string httpResponseMessage)
        where TErrorMapper : IApiErrorMapper
    {
        string message = null;

        if (errorRes.Message is string singleMessage)
        {
            message = singleMessage;
        }
        else if (errorRes.Message is System.Text.Json.JsonElement messageElement)
        {
            var messages = messageElement.EnumerateArray().Select(m => m.GetString())
                .Where(m => !string.IsNullOrEmpty(m));
            message = string.Join(", ", messages);
        }

        if (message == null)
        {
            message = httpResponseMessage.ToObject<TErrorMapper>().ErrorMessageGetter;
        }

        if (message != null && errorRes.Code == 0 && string.IsNullOrWhiteSpace(errorRes.ErrorMessage) &&
            (errorRes.Message == null || errorRes.Message?.ToString() == ""))
        {
            errorRes.ErrorMessage = message;
        }

        return message;
    }

    private string AddQueryString(string uri, Dictionary<string, string[]> queries)
    {
        if (!queries.Any())
            return uri;
        var queryString = string.Join("&",
            queries.Select(p => $"{p.Key}={string.Join(",", p.Value)}"));
        var url = $"{uri}?{queryString}";

        return url;
    }
}

public sealed class DefaultErrorMapper : IApiErrorMapper
{
    public string ErrorMessageGetter => null;
}