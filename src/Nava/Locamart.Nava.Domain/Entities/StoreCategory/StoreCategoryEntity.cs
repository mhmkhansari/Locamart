using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Domain.Entities.StoreCategory.Enums;
using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;

namespace Locamart.Nava.Domain.Entities.StoreCategory;

public sealed class StoreCategoryEntity : Dina.Entity<StoreCategoryId>
{
    public string Name { get; private set; }
    public StoreCategoryId? ParentId { get; private set; }
    public StoreCategoryStatus Status { get; private set; }
    public static Result<StoreCategoryEntity, Error> Create(string name, StoreCategoryId? parentId)
    {
        var storeCategoryId = StoreCategoryId.Create(Guid.NewGuid());

        return new StoreCategoryEntity(storeCategoryId.Value, name, parentId);
    }

    private StoreCategoryEntity(StoreCategoryId id, string name, StoreCategoryId? parentId = null) : base(id)
    {
        Name = name;
        ParentId = parentId;
        Status = StoreCategoryStatus.Enabled;
    }
}

