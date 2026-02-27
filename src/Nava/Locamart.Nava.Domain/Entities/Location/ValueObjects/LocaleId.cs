using CSharpFunctionalExtensions;
using Locamart.Dina;

namespace Locamart.Nava.Domain.Entities.Location.ValueObjects;

public sealed class LocaleId : ValueObject<LocaleId>, IComparable<LocaleId>
{
    public Guid Value { get; }

    private LocaleId(Guid value)
    {
        Value = value;
    }

    public static Result<LocaleId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("locale_id_not_valid", "Locale Id cannot be empty");

        return new LocaleId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(LocaleId orderId) => orderId.Value;
    protected override bool EqualsCore(LocaleId other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(LocaleId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}
