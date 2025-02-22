namespace Core.CryptoExchangeRate.Application.Framework.Commands;

public interface ICommandBaseRepository<TEntity, T>
{
    Task<TEntity> GetByIdAsync(T id, CancellationToken cancellationToken = default);
    Task Add(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRange(List<TEntity> entity, CancellationToken cancellationToken = default);
    Task Attach(TEntity entity, CancellationToken cancellationToken = default);
    Task AttachRange(List<TEntity> entity, CancellationToken cancellationToken = default);
    Task Update(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateRange(TEntity entity, CancellationToken cancellationToken = default);
    Task Remove(TEntity entity, CancellationToken cancellationToken = default);
    
}
