
using CSharpFunctionalExtensions;

namespace Locamart.Domain.StoreCategory.ValueObjects;

public sealed class StoreCategoryId : ValueObject<StoreCategoryId>
{
    public Guid? Value { get; private set; }

    private StoreCategoryId(Guid? value)
    {
        Value = value;
    }

    public static StoreCategoryId? Create(Guid? value) =>
        value.HasValue ? new StoreCategoryId(value) : null;

    public static StoreCategoryId New() => new(Guid.NewGuid());

    public override string ToString() => Value?.ToString() ?? string.Empty;

    public static implicit operator Guid?(StoreCategoryId? storeCategoryId) =>
        storeCategoryId?.Value;

    protected override bool EqualsCore(StoreCategoryId? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value?.GetHashCode() ?? 0;
    }
}

