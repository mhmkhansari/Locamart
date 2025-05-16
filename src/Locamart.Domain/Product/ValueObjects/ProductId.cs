using CSharpFunctionalExtensions;

namespace Locamart.Domain.Product.ValueObjects;

public sealed class ProductId : ValueObject<ProductId>
{
    public Guid Value { get; private set; }

    private ProductId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(value));

        Value = value;
    }

    public static ProductId Create(Guid value) => new(value);

    public static ProductId New() => new(Guid.NewGuid());

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
}