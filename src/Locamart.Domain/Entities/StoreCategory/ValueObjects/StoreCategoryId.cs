
using CSharpFunctionalExtensions;

namespace Locamart.Domain.Entities.StoreCategory.ValueObjects;

public sealed class StoreCategoryId : ValueObject<StoreCategoryId>, IComparable<StoreCategoryId>
{
    public Guid Value { get; private set; }

    private StoreCategoryId(Guid value)
    {
        Value = value;
    }

    public static StoreCategoryId Create(Guid value) =>
         new StoreCategoryId(value);

    public static StoreCategoryId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(StoreCategoryId storeCategoryId) =>
        storeCategoryId.Value;

    protected override bool EqualsCore(StoreCategoryId other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(StoreCategoryId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}

