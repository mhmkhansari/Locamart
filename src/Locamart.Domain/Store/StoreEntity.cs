using CSharpFunctionalExtensions;
using Locamart.Domain.Store.RequestModels;
using Locamart.Domain.Store.ValueObjects;
using Locamart.Domain.StoreCategory.ValueObjects;
using Locamart.Shared;
using Locamart.Shared.ValueObjects;

namespace Locamart.Domain.Store;

public sealed class StoreEntity : Shared.Entity<StoreId>
{
    public string Name { get; private set; }
    public StoreCategoryId CategoryId { get; private set; }
    public Image? ProfileImage { get; private set; }
    public Location? Location { get; private set; }
    public string? Bio { get; private set; }
    public Uri? Website { get; private set; }


    public static Result<StoreEntity, Error> Create(CreateStoreRequest request)
    {
        return new StoreEntity(StoreId.Create(Guid.NewGuid()), request.Name, request.CategoryId);
    }

    private StoreEntity(StoreId id, string name, StoreCategoryId categoryId) : base(id)
    {
        Name = name;
        CategoryId = categoryId;
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
}

