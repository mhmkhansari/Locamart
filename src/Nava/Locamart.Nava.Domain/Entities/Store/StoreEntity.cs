using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.Enums;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;
using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Store;

public sealed class StoreEntity : AuditableEntity<StoreId>
{
    public string Name { get; private set; }
    public StoreCategoryId CategoryId { get; private set; }
    public Image? ProfileImage { get; private set; }
    public Location? Location { get; private set; }
    public string? Bio { get; private set; }
    public Uri? Website { get; private set; }
    public StoreIdentifier? Identifier { get; private set; }
    public StoreStatus Status { get; private set; }


    private StoreEntity() : base() { }
    public static Result<StoreEntity, Error> Create(string name, StoreCategoryId categoryId, UserId ownerId)
    {
        var storeId = StoreId.Create(Guid.NewGuid());

        if (storeId.IsFailure)
            return storeId.Error;

        return new StoreEntity(storeId.Value, ownerId, name, categoryId);
    }

    private StoreEntity(StoreId id, UserId ownerId, string name, StoreCategoryId categoryId) : base(id)
    {
        Name = name;
        CategoryId = categoryId;
        Status = StoreStatus.Open;
    }

    public void SetProfileImage(Image image)
    {
        ProfileImage = image;
    }

    public void SetBio(string bio)
    {
        Bio = bio;
    }

    public void SetWebsite(Uri website)
    {
        Website = website;
    }

    public void SetLocation(Location location)
    {
        Location = location;
    }

    public void SetStoreIdentifier(StoreIdentifier storeIdentifier)
    {
        Identifier = storeIdentifier;
    }
}

