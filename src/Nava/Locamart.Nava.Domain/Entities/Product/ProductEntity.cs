using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Product.Enums;
using Locamart.Nava.Domain.Entities.Product.RequestModels;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Product;

public sealed class ProductEntity : Dina.Entity<ProductId>
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public Price Price { get; private set; }
    public List<Image> Images { get; } = [];
    public StoreId StoreId { get; private set; }
    public UserId CreatedBy { get; private set; }
    public ProductStatus Status { get; private set; }
    public List<string> Tags { get; private set; }

    private ProductEntity() : base(default!) { }

    private ProductEntity(ProductId id, StoreId storeId, UserId createdBy, string title, string description, Price price, List<Image> images, List<string> tags) : base(id)
    {
        StoreId = storeId;
        CreatedBy = createdBy;
        Title = title;
        Description = description;
        Price = price;
        Status = ProductStatus.Available;
        Tags = tags;

        AddImages(images);

        if (!string.IsNullOrEmpty(description))
            SetDescription(description);
        Tags = tags;
    }

    public static Result<ProductEntity, Error> Create(AddProductRequest request)
    {
        var productId = ProductId.Create(Guid.NewGuid());

        if (productId.IsFailure)
            return productId.Error;

        var storeId = StoreId.Create(request.StoreId);

        if (storeId.IsFailure)
            return storeId.Error;

        var createdById = UserId.Create(request.CreatedBy);

        if (createdById.IsFailure)
            return createdById.Error;

        var price = Price.Create(request.Price, "IRR");

        if (price.IsFailure)
            return price.Error;

        if (request.Tags.Count > 5)
            return Error.Create("too_many_tags", "Too many tags");

        return new ProductEntity(productId.Value, storeId.Value, createdById.Value, request.Title, request.Description, price.Value, request.Images, request.Tags);

    }

    public UnitResult<Error> SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Error.Create("product_title_cannot_be_empty", "Product title cannot be empty");

        Title = title;

        return UnitResult.Success<Error>();
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void SetPrice(Price price)
    {
        Price = price;
    }

    public UnitResult<Error> AddImages(IEnumerable<Image> images)
    {
        Images.AddRange(images);
        return UnitResult.Success<Error>();
    }

    public void SetStatus(ProductStatus status)
    {
        Status = status;
    }

    public override string ToString() =>
        $"Product [{Id}] - {Title} (${Price:F2})";
}