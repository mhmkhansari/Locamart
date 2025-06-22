namespace Locamart.Adapter.Http.StoreCategory.RequestModels;

public record CreateStoreCategoryRequest
{
    public string Name { get; init; }
    public Guid? ParentId { get; init; }
}

