
namespace Locamart.Nava.Domain.Entities.Comment.RequestModels;

public class AddCommentAttachmentRequest
{
    public Guid CommentId { get; set; }
    public Uri Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

