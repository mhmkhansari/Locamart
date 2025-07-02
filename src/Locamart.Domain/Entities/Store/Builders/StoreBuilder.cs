using CSharpFunctionalExtensions;
using Locamart.Domain.Entities.Store;
using Locamart.Domain.Entities.StoreCategory.ValueObjects;
using Locamart.Shared;
using Locamart.Shared.ValueObjects;

namespace Locamart.Domain.Entities.Store.Builders;

public class StoreBuilder(string name, StoreCategoryId categoryId)
{
    private readonly List<Action<StoreEntity>> _configurations = [];

    public StoreBuilder Configure(Action<StoreEntity> configure)
    {
        _configurations.Add(configure);
        return this;
    }

    public StoreBuilder WithProfileImage(Image image) =>
        Configure(store => store.SetProfileImage(image));

    public StoreBuilder WithBio(string bio) =>
        Configure(store => store.SetBio(bio));

    public StoreBuilder WithWebsite(Uri website) =>
        Configure(store => store.SetWebsite(website));

    public StoreBuilder WithLocation(Location location) =>
        Configure(store => store.SetLocation(location));

    public Result<StoreEntity, Error> Build()
    {
        var result = StoreEntity.Create(name, categoryId);
        if (result.IsFailure)
            return result.Error;

        var store = result.Value;

        foreach (var configure in _configurations)
            configure(store);

        return store;
    }
}
