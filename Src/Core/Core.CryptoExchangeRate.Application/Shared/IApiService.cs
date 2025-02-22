using Core.CryptoExchangeRate.Application.Shared.Models;

namespace Core.CryptoExchangeRate.Application.Shared;

public interface IApiService<T>
{
    public Task<TRes> CallApi<TRes>(ApiParameter apiParameter, CancellationToken cancellationToken = new())
        where TRes : ServiceResContextBase, new();

    public Task<TRes> CallApi<TRes, TErrorMapper>(ApiParameter apiParameter,
        CancellationToken cancellationToken = new())
        where TRes : ServiceResContextBase, new()
        where TErrorMapper : IApiErrorMapper;
}