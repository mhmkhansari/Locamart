using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.Product.AddComment;

public class AddCommentCommand : ICommand<UnitResult<Error>>
{
    public Guid ProductId { get; set; }
    public Guid? ParentId { get; set; }
    public string BodyMarkdown { get; set; }
    public IEnumerable<AddCommentAttachmentDto>? Attachments { get; set; }

}

public class AddCommentAttachmentDto
{
    public Uri Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

