using Core.CryptoExchangeRate.Application.Framework.Queries;
using Core.CryptoExchangeRate.Application.Shared;
using Core.CryptoExchangeRate.Domain.Framework;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Core.CryptoExchangeRate.Application.Framework.Behavior;

public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IQuery<TResponse> where TResponse : class
{
    private readonly IMemoryCache _memoryCache;

    public CachingBehavior(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        if (request is ICacheableRequest<TResponse> cacheable)
        {
            var cacheKey = cacheable.CacheKey;
            ArgumentException.ThrowIfNullOrEmpty(cacheKey);


            var response = _memoryCache.Get<Result<TResponse>>(cacheKey);

            if (response is not null) return response;

            response = await next().ConfigureAwait(false);

            var conditionFroSetCacheMemory = cacheable.ConditionFroSetCache;
            if (response is not null && !response.IsSuccess &&
                (conditionFroSetCacheMemory is null || conditionFroSetCacheMemory(response.Value)))
                _memoryCache.Set(cacheKey, response, cacheable.ConditionExpiration(response.Value));

            return response;
        }

        return await next().ConfigureAwait(false);
    }
}

