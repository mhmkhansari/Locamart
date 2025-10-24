using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Utils;
using Locamart.Nava.Domain.Entities.Comment.Enums;
using Locamart.Nava.Domain.Entities.Comment.ValueObjects;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Comment;

public class CommentEntity : AuditableEntity<CommentId>
{

    public ProductId ProductId { get; private set; }
    public CommentId? ParentId { get; private set; }
    public CommentStatus Status { get; private set; }
    public string BodyMarkdown { get; private set; }
    public bool IsEdited { get; private set; }
    private readonly List<CommentAttachmentEntity> _attachments = new();
    public IReadOnlyCollection<CommentAttachmentEntity> Attachments => _attachments.AsReadOnly();
    private CommentEntity(CommentId id) : base(id) { }


    public static Result<CommentEntity, Error> Create(ProductId productId, CommentId? parentId, string bodyMarkdown)
    {
        var commentId = CommentId.Create(DinaGuid.NewSequentialGuid());

        if (commentId.IsFailure)
            return commentId.Error;

        return new CommentEntity(commentId.Value, productId, parentId, bodyMarkdown);

    }
    private CommentEntity(CommentId id, ProductId productId, CommentId? parentId, string bodyMarkdown) : base(id)
    {
        ProductId = productId;
        ParentId = parentId;
        BodyMarkdown = bodyMarkdown;
        Status = CommentStatus.Published;
    }
    public UnitResult<Error> AddAttachment(CommentAttachmentEntity attachment)
    {
        const int maxAttachments = 5;

        if (_attachments.Count >= maxAttachments)
            return Error.Create("max_attachment_exceeds", $"Cannot add more than {maxAttachments} attachments.");

        if (attachment.CommentId != Id)
            return Error.Create("attachment_already_belongs_another_comment", "Attachment already belongs to another comment.");

        attachment.AttachTo(Id);

        _attachments.Add(attachment);

        return UnitResult.Success<Error>();
    }

    public void AddAttachments(IEnumerable<CommentAttachmentEntity> attachments)
    {
        foreach (var a in attachments) AddAttachment(a);
    }
}

