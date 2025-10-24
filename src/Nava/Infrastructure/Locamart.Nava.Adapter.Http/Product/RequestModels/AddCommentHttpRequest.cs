namespace Locamart.Nava.Adapter.Http.Product.RequestModels;

public class AddCommentHttpRequest
{
    public Guid ProductId { get; set; }
    public Guid? ParentId { get; set; }
    public string BodyMarkdown { get; set; }
    public IEnumerable<AddCommentAttachmenHttpRequest>? Attachments { get; set; }

}

public class AddCommentAttachmenHttpRequest
{
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}


