using Locamart.Nava.Application.Contracts;
using Locamart.Nava.Application.Contracts.Dtos;
using Locamart.Nava.Application.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.QueryServices;

public class CommentQueryService(LocamartNavaQueryDbContext dbContext) : ICommentQueryService
{
    public async Task<PagedResult<CommentDto>> GetComments(Guid productId, Guid? cursor, int pageSize = 100)
    {
        var query = dbContext.Comments
            .Where(c => c.ProductId == productId);

        if (cursor.HasValue)
            query = query.Where(c => c.Id.CompareTo(cursor.Value) > 0);

        var items = await query.Take(pageSize + 1)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                BodyMarkdown = c.BodyMarkdown
            })
            .ToListAsync();

        bool hasNext = items.Count > pageSize;
        Guid? nextCursor = hasNext ? items[^1].Id : null;

        if (hasNext)
            items.RemoveAt(pageSize);

        return new PagedResult<CommentDto>(items, nextCursor, hasNext);
    }
}

