using Locamart.Nava.Application.Contracts.Dtos;

namespace Locamart.Nava.Application.Contracts.Services;

public interface ICommentQueryService
{

    public Task<PagedResult<CommentDto>> GetComments(Guid productId, Guid? cursor, int pageSize = 100);
}

