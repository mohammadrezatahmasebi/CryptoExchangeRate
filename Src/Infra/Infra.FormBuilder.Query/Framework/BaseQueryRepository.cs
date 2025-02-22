using Si24.Core.FormBuilder.Domain.Framework;
using Si24.Infra.FormBuilder.Query.Configs.Contexts;

namespace Si24.Infra.FormBuilder.Query.Framework;

public abstract class BaseQueryRepository<TEntity, TId>(FormBuilderQueryContext dbContext)
    where TEntity : BaseEntity<TId>
{
    protected readonly FormBuilderQueryContext _dbContext = dbContext;

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
    }

}