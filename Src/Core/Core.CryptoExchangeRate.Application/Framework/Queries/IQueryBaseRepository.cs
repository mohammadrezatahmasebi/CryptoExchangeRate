namespace Core.CryptoExchangeRate.Application.Framework.Queries;

public interface IQueryBaseRepository<TEntity, T>
{
    Task<TEntity?> GetByIdAsync(T id, CancellationToken cancellationToken = default);

}
