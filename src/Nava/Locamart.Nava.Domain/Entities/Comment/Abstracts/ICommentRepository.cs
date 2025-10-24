using Locamart.Nava.Domain.Entities.Comment.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Comment.Abstracts;

public interface ICommentRepository
{
    Task AddAsync(CommentEntity comment, CancellationToken cancellationToken);
    Task<CommentEntity?> GetByIdAsync(CommentId id, CancellationToken cancellationToken);
    void Remove(CommentEntity comment);
    void Update(CommentEntity comment);
}

