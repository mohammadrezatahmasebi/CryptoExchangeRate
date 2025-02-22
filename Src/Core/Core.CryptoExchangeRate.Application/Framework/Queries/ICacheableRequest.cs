namespace Core.CryptoExchangeRate.Application.Framework.Queries;

public interface ICacheableRequest<T> where T : class
{
    string CacheKey { get; }
    Func<T, DateTimeOffset> ConditionExpiration { get; }
    virtual Func<bool> ConditionFroGetCache => null;
    virtual Func<T, bool> ConditionFroSetCache => null;

}
public interface ICacheInvalidatorRequest
{
    string CacheKey { get; }
    bool UseMemoryCache { get; }
}

