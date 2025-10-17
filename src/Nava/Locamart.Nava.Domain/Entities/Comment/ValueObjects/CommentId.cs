using CSharpFunctionalExtensions;
using Locamart.Dina;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Locamart.Nava.Domain.Entities.Comment.ValueObjects;

public sealed class CommentId : ValueObject, IComparable<CommentId>
{
    public Guid Value { get; }

    private CommentId(Guid value)
    {
        Value = value;
    }

    public static Result<CommentId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("comment_id_not_valid", "Store Id cannot be empty");

        return new CommentId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(CommentId productId) => productId.Value;

    public int CompareTo(CommentId? other)
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