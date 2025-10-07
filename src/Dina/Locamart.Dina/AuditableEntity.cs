using Locamart.Dina.Abstracts;
using Locamart.Dina.ValueObjects;

namespace Locamart.Dina;

public abstract class AuditableEntity<TId> : Entity<TId>, IAuditable
    where TId : notnull
{
    protected AuditableEntity() { }
    protected AuditableEntity(TId id) : base(id) { }
    public DateTime CreatedAt { get; protected set; }
    public UserId CreatedBy { get; protected set; }
    public DateTime? LastUpdatedAt { get; protected set; }
    public UserId? UpdatedBy { get; protected set; }

    public void SetCreated(DateTime now, UserId user)
    {
        CreatedAt = now;
        CreatedBy = user;
    }

    public void SetUpdated(DateTime now, UserId? user)
    {
        LastUpdatedAt = now;
        UpdatedBy = user;
    }
}
