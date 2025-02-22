namespace Core.CryptoExchangeRate.Domain.Framework;

public abstract class BaseEntity<TId>
{
    protected BaseEntity(TId id)
    {
        Id = id;
    }
    protected BaseEntity()
    {

    }
    public TId Id { get; init; }
    public int CreatorId { get; set; }
    public int? ModifierId { get; set; }
    public DateTime CreatedAt { get; private set; } = TimeProvider.System.GetUtcNow().DateTime;
    public DateTime ModifiedAt { get; set; } = TimeProvider.System.GetUtcNow().DateTime;

}
