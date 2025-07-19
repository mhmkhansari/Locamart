using CSharpFunctionalExtensions;
using Locamart.Dina;
using ValueObject = Locamart.Dina.ValueObject;

namespace Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;

public sealed class StoreCategoryId : ValueObject
{
    public Guid Value { get; private set; }

    private StoreCategoryId(Guid value)
    {
        Value = value;
    }

    public static Result<StoreCategoryId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("store_category_id_not_valid", "StoreCategory Id cannot be empty");

        return new StoreCategoryId(value);
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(StoreCategoryId storeCategoryId) =>
        storeCategoryId.Value;

    public int CompareTo(StoreCategoryId? other)
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

