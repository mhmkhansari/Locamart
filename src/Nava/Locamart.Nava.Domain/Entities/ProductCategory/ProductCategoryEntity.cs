using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Domain.Entities.ProductCategory.Enums;
using Locamart.Nava.Domain.Entities.ProductCategory.ValueObjects;

namespace Locamart.Nava.Domain.Entities.ProductCategory;

public sealed class ProductCategoryEntity : AuditableEntity<ProductCategoryId>
{
    public string Name { get; private set; }
    public ProductCategoryId? ParentId { get; private set; }
    public ProductCategoryStatus Status { get; private set; }

    private ProductCategoryEntity() : base() { }

    private ProductCategoryEntity(
        ProductCategoryId id,
        string name,
        ProductCategoryId? parentId)
        : base(id)
    {
        Name = name;
        ParentId = parentId;
        Status = ProductCategoryStatus.Enabled;
    }

    public static Result<ProductCategoryEntity, Error> Create(
        string name,
        ProductCategoryId? parentId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Create(
                "product_category_name_required",
                "Product category name cannot be empty");

        var idResult = ProductCategoryId.Create(Guid.NewGuid());
        if (idResult.IsFailure)
            return idResult.Error;

        return new ProductCategoryEntity(
            idResult.Value,
            name.Trim(),
            parentId);
    }


    public UnitResult<Error> SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Create(
                "product_category_name_required",
                "Product category name cannot be empty");

        Name = name.Trim();
        return UnitResult.Success<Error>();
    }

    public void SetParent(ProductCategoryId? parentId)
    {
        ParentId = parentId;
    }

    public void Enable()
    {
        Status = ProductCategoryStatus.Enabled;
    }

    public void Disable()
    {
        Status = ProductCategoryStatus.Disabled;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}

