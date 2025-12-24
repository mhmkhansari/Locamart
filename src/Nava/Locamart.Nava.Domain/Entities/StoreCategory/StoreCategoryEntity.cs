using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Utils;
using Locamart.Nava.Domain.Entities.StoreCategory.Enums;
using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;

namespace Locamart.Nava.Domain.Entities.StoreCategory;

public sealed class StoreCategoryEntity : AuditableEntity<StoreCategoryId>
{
    public string Name { get; private set; }
    public StoreCategoryId? ParentId { get; private set; }
    public StoreCategoryStatus Status { get; private set; }

    private StoreCategoryEntity(StoreCategoryId id) : base(id) { }
    public static Result<StoreCategoryEntity, Error> Create(string name, StoreCategoryId? parentId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Create(
            "store_category_name_required",
            "Store category name cannot be empty");


        var storeCategoryId = StoreCategoryId.Create(DinaGuid.NewSequentialGuid());

        if (storeCategoryId.IsFailure)
            return storeCategoryId.Error;

        return new StoreCategoryEntity(storeCategoryId.Value, name.Trim(), parentId);
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

