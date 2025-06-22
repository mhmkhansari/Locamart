using Locamart.Domain.StoreCategory.ValueObjects;
using Locamart.Shared;

namespace Locamart.Domain.StoreCategory;

public sealed class StoreCategoryEntity : Entity<StoreCategoryId>
{
    public string Name { get; private set; }
    public StoreCategoryId? ParentId { get; private set; }

    public static StoreCategoryEntity Create(string name, StoreCategoryId? parentId)
    {
        var storeCategoryId = StoreCategoryId.Create(Guid.NewGuid());

        return new StoreCategoryEntity(storeCategoryId, name, parentId);
    }

    private StoreCategoryEntity(StoreCategoryId id, string name, StoreCategoryId? parentId = null) : base(id)
    {
        Name = name;
        ParentId = parentId;
    }
}

