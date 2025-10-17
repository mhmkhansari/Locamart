using Locamart.Nava.Domain.Entities.Comment.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Comment.Abstracts;

public interface ICommentAttachmentRepository
{
    Task AddAsync(CommentAttachmentEntity commentAttachment);
    Task<CommentAttachmentEntity> GetByIdAsync(CommentAttachmentId id);
    Task<IEnumerable<CommentAttachmentEntity>> GetAllAsync();
    void Update(CommentAttachmentEntity commentAttachment);
    void Delete(CommentAttachmentEntity commentAttachment);
}

