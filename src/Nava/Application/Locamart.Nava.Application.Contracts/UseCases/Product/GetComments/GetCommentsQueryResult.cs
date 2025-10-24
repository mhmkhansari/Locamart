using Locamart.Nava.Application.Contracts.Dtos;

namespace Locamart.Nava.Application.Contracts.UseCases.Product.GetComments;

public class GetCommentsQueryResult
{
    public IEnumerable<CommentDto> Comments { get; set; }
}

