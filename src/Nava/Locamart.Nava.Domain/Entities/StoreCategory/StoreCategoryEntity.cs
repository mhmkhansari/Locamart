using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Domain.Entities.StoreCategory.Enums;
using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;

namespace Locamart.Nava.Domain.Entities.StoreCategory;

public sealed class StoreCategoryEntity : AuditableEntity<StoreCategoryId>
{
    public string Name { get; private set; }
    public StoreCategoryId? ParentId { get; private set; }
    public StoreCategoryStatus Status { get; private set; }

    private StoreCategoryEntity() : base() { }
    public static Result<StoreCategoryEntity, Error> Create(string name, StoreCategoryId? parentId)
    {
        var storeCategoryId = StoreCategoryId.Create(Guid.NewGuid());

        return new StoreCategoryEntity(storeCategoryId.Value, name, parentId);
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetParentId(StoreCategoryId parentId)
    {
        ParentId = parentId;
    }

    private StoreCategoryEntity(StoreCategoryId id, string name, StoreCategoryId? parentId = null) : base(id)
    {
        Name = name;
        ParentId = parentId;
        Status = StoreCategoryStatus.Enabled;
    }
}

