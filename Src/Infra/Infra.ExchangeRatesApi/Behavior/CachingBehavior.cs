using Core.CryptoExchangeRate.Application.Framework.Queries;
using Core.CryptoExchangeRate.Application.Shared;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Infra.ExchangeRatesApi.Behavior;

public sealed class CachingBehavior<TRequest, TResponse> : BehaviorBase<TRequest, TResponse>
    where TRequest : IQuery<TResponse>, ICacheableRequest<TResponse>
    where TResponse : ServiceResContextBase, new()
{
    private readonly IMemoryCache _memoryCache;

    public CachingBehavior(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    protected override async Task<TResponse> HandleCore(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (request is ICacheableRequest<TResponse> cacheable)
        {
            var cacheKey = cacheable.CacheKey;
            ArgumentException.ThrowIfNullOrEmpty(cacheKey);


            var response = _memoryCache.Get<TResponse>(cacheKey);

            if (response is not null) return response;

            response = await next().ConfigureAwait(false);

            var conditionFroSetCacheMemory = cacheable.ConditionFroSetCache;
            if (response is not null && !response.HasError &&
                (conditionFroSetCacheMemory is null || conditionFroSetCacheMemory(response)))
                _memoryCache.Set(cacheKey, response, cacheable.ConditionExpiration(response));

            return response;
        }

        return await next().ConfigureAwait(false);
    }
}

public abstract class BehaviorBase<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>,
    IRequest<TResult>
    where TResult : ServiceResContextBase, new()
{
    public Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        return HandleCore(request, cancellationToken, next);
    }

    protected TResult Failure(ValidationError error)
    {
        return new TResult { ValidationError = error };
    }

    protected TResult Failure(string code, string message)
    {
        return new TResult { ValidationError = new ValidationError(code, message) };
    }

#pragma warning disable CA1068 // CancellationToken parameters must come last
    protected abstract Task<TResult> HandleCore(TRequest request, CancellationToken cancellationToken,
#pragma warning restore CA1068 // CancellationToken parameters must come last
        RequestHandlerDelegate<TResult> next);
}