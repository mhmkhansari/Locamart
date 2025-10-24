using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Dtos;

namespace Locamart.Nava.Application.Contracts.UseCases.Product.GetComments;

public class GetCommentsQuery : IQuery<Result<PagedResult<CommentDto>, Error>>
{
    public Guid ProductId { get; set; }
    public Guid? Cursor { get; set; }
    public int? PageSize { get; set; }
}

