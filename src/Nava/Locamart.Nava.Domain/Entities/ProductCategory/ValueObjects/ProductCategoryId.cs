using CSharpFunctionalExtensions;
using Locamart.Dina;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Locamart.Nava.Domain.Entities.ProductCategory.ValueObjects;

public sealed class ProductCategoryId : ValueObject, IComparable<ProductCategoryId>
{
    public Guid Value { get; }

    private ProductCategoryId(Guid value)
    {
        Value = value;
    }

    public static Result<ProductCategoryId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("product_id_not_valid", "Store Id cannot be empty");

        return new ProductCategoryId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(ProductCategoryId productCategoryId) => productCategoryId.Value;

    public int CompareTo(ProductCategoryId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
    public static ProductCategoryId Empty() => new(Guid.Empty);
}

