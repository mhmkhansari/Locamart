namespace Locamart.Nava.Application.Contracts.Dtos;

public class CommentDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string BodyMarkdown { get; set; }
}

