using CSharpFunctionalExtensions;
using Locamart.Domain.Store.RequestModels;
using Locamart.Domain.Store.ValueObjects;
using Locamart.Shared;

namespace Locamart.Domain.Store;

public sealed class StoreEntity : Shared.Entity<StoreId>
{
    public string Name { get; private set; }
    public string? ProfileImage { get; private set; }
    public Location Location { get; private set; }
    public string Bio { get; private set; }
    public Uri Website { get; private set; }


    public static Result<StoreEntity, Error> Create(CreateStoreRequest request)
    {
        return new StoreEntity(StoreId.Create(Guid.NewGuid()), request.Name);
    }

    private StoreEntity(StoreId id, string name) : base(id)
    {
        Name = name;
    }

    public void AddProfileImage()
}

