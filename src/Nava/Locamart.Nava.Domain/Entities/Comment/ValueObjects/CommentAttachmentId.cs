using CSharpFunctionalExtensions;
using Locamart.Dina;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Locamart.Nava.Domain.Entities.Comment.ValueObjects;

public class CommentAttachmentId : ValueObject, IComparable<CommentAttachmentId>
{
    public Guid Value { get; }

    private CommentAttachmentId(Guid value)
    {
        Value = value;
    }

    public static Result<CommentAttachmentId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("comment_attachment_id_not_valid", "Store Id cannot be empty");

        return new CommentAttachmentId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(CommentAttachmentId productId) => productId.Value;

    public int CompareTo(CommentAttachmentId? other)
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

