using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Store.RequestModels;

public class CreateStoreRequest
{
    public string Name { get; init; }
    public StoreCategoryId CategoryId { get; init; }
}

