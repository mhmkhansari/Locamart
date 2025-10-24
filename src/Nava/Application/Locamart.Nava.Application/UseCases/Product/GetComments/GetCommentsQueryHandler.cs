using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts;
using Locamart.Nava.Application.Contracts.Dtos;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.Product.GetComments;

namespace Locamart.Nava.Application.UseCases.Product.GetComments;

public class GetCommentsQueryHandler(ICommentQueryService commentQueryService) : IQueryHandler<GetCommentsQuery, Result<PagedResult<CommentDto>, Error>>
{
    public async Task<Result<PagedResult<CommentDto>, Error>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var commentsResult = await commentQueryService.GetComments(request.ProductId, request.Cursor, request.PageSize ?? 100);

        return Result.Success<PagedResult<CommentDto>, Error>(commentsResult);
    }
}

