using CSharpFunctionalExtensions;
using Locamart.Shared;

namespace Locamart.Domain.Entities.Product.ValueObjects;

public sealed class ProductId : ValueObject<ProductId>, IComparable<ProductId>
{
    public Guid Value { get; private set; }

    private ProductId(Guid value)
    {
        Value = value;
    }

    public static Result<ProductId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("product_id_not_valid", "Store Id cannot be empty");

        return new ProductId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(ProductId productId) => productId.Value;
    protected override bool EqualsCore(ProductId other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(ProductId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}