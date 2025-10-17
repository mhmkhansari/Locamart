using Locamart.Nava.Domain.Entities.Comment;
using Locamart.Nava.Domain.Entities.Comment.Abstracts;
using Locamart.Nava.Domain.Entities.Comment.ValueObjects;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

public class CommentRepository(LocamartNavaDbContext dbContext) : ICommentRepository
{
    public async Task AddAsync(CommentEntity comment, CancellationToken cancellationToken)
    {
        await dbContext.Comments.AddAsync(comment, cancellationToken);
    }

    public Task<CommentEntity> GetByIdAsync(CommentId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Remove(CommentEntity comment)
    {
        dbContext.Comments.Remove(comment);
    }
}

