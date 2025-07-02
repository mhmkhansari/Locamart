using Locamart.Domain.Entities.StoreCategory.ValueObjects;

namespace Locamart.Domain.Entities.Store.RequestModels;

public class CreateStoreRequest
{
    public string Name { get; init; }
    public StoreCategoryId CategoryId { get; init; }
}

