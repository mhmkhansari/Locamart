using CSharpFunctionalExtensions;
using Locamart.Dina;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Locamart.Nava.Domain.Entities.UserAddress.ValueObjects;

public sealed class UserAddressId : ValueObject
{
    public Guid Value { get; private set; }

    private UserAddressId(Guid value)
    {
        Value = value;
    }

    public static Result<UserAddressId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("store_category_id_not_valid", "StoreCategory Id cannot be empty");

        return new UserAddressId(value);
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(UserAddressId storeCategoryId) =>
        storeCategoryId.Value;

    public int CompareTo(UserAddressId? other)
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

