using Locamart.Domain.StoreCategory.ValueObjects;

namespace Locamart.Domain.Store.RequestModels;

public class CreateStoreRequest
{
    public string Name { get; init; }
    public StoreCategoryId CategoryId { get; init; }
}

