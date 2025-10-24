using Locamart.Nava.Domain.Entities.Comment;
using Locamart.Nava.Domain.Entities.Comment.Abstracts;
using Locamart.Nava.Domain.Entities.Comment.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

internal sealed class CommentRepository(LocamartNavaDbContext dbContext) : ICommentRepository
{
    public async Task AddAsync(CommentEntity comment, CancellationToken cancellationToken)
    {
        await dbContext.Comments.AddAsync(comment, cancellationToken);
    }

    public Task<CommentEntity?> GetByIdAsync(CommentId id, CancellationToken cancellationToken)
    {
        return dbContext.Comments
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public void Remove(CommentEntity comment)
    {
        dbContext.Comments.Remove(comment);
    }

    public void Update(CommentEntity comment)
    {
        dbContext.Comments.Update(comment);
    }
}

