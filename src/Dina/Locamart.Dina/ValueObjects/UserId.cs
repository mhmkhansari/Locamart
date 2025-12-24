using CSharpFunctionalExtensions;

namespace Locamart.Dina.ValueObjects;

public sealed class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId() { }

    private UserId(Guid value)
    {
        Value = value;
    }

    public static Result<UserId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("user_id_not_valid", "User Id cannot be empty");

        return new UserId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(UserId userId) => userId.Value;

    public int CompareTo(UserId? other)
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

