using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;


namespace Locamart.Nava.Domain.Entities.Store.Builders;

public class StoreBuilder(string name, StoreCategoryId categoryId, UserId ownerId)
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
        var result = StoreEntity.Create(name, categoryId, ownerId);
        if (result.IsFailure)
            return result.Error;

        var store = result.Value;

        foreach (var configure in _configurations)
            configure(store);

        return store;
    }
}
