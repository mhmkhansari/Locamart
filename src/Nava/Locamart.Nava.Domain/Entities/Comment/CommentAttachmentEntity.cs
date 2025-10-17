using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Domain.Entities.Comment.Enums;
using Locamart.Nava.Domain.Entities.Comment.RequestModels;
using Locamart.Nava.Domain.Entities.Comment.ValueObjects;


namespace Locamart.Nava.Domain.Entities.Comment;

public class CommentAttachmentEntity : AuditableEntity<CommentAttachmentId>
{
    public CommentId CommentId { get; private set; }
    public CommentAttachmentKind AttachmentKind { get; private set; }
    public Uri Url { get; private set; }
    public string ContentType { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    private CommentAttachmentEntity(CommentAttachmentId id) : base(id) { }
    public static Result<CommentAttachmentEntity, Error> Create(AddCommentAttachmentRequest request)
    {

        var attachmentId = CommentAttachmentId.Create(Guid.NewGuid());
        if (attachmentId.IsFailure)
            return attachmentId.Error;

        var commentIdValue = ValueObjects.CommentId.Create(request.CommentId);
        if (commentIdValue.IsFailure)
            return commentIdValue.Error;

        return new CommentAttachmentEntity(attachmentId.Value, commentIdValue.Value, CommentAttachmentKind.Image, request.Url, "Image", request.Width, request.Height);
    }

    private CommentAttachmentEntity(CommentAttachmentId id, CommentId commentId, CommentAttachmentKind attachmentKind, Uri url, string contentType, int width, int height) : base(id)
    {
        CommentId = commentId;
        AttachmentKind = attachmentKind;
        Url = url;
        ContentType = contentType;
        Width = width;
        Height = height;
    }

    internal UnitResult<Error> AttachTo(CommentId commentId)
    {
        if (!CommentId.Equals(commentId))
            return Error.Create("attachment_conflict", "Attachment already attached to a different comment.");

        CommentId = commentId;
        return UnitResult.Success<Error>();
    }

}

