using CSharpFunctionalExtensions;
using Locamart.Dina;
using ValueObject = Locamart.Dina.ValueObject;

namespace Locamart.Nava.Domain.Entities.Product.ValueObjects;

public sealed class ProductId : ValueObject, IComparable<ProductId>
{
    public Guid Value { get; }

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

    public int CompareTo(ProductId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}