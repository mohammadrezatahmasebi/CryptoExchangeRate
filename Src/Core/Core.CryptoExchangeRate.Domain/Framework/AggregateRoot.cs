namespace Core.CryptoExchangeRate.Domain.Framework;

public abstract class AggregateRoot<TId> : BaseEntity<TId>
{
    protected AggregateRoot(TId id) : base(id)
    {

    }
    protected AggregateRoot() : base()
    {

    }
}
