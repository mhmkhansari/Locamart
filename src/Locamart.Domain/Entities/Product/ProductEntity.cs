using CSharpFunctionalExtensions;
using Locamart.Domain.Entities.Product.RequestModels;
using Locamart.Domain.Entities.Product.ValueObjects;
using Locamart.Domain.Entities.Store.ValueObjects;
using Locamart.Shared;
using Locamart.Shared.ValueObjects;

namespace Locamart.Domain.Entities.Product;

public sealed class ProductEntity : Entity<ProductId>
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public Price Price { get; private set; }
    public List<Image> Images { get; } = [];
    public StoreId StoreId { get; init; }

    private ProductEntity() : base(default!) { }

    private ProductEntity(ProductId id, string title, string description, Price price, List<Image> images) : base(id)
    {
        Title = title;
        Description = description;
        Price = price;
        AddImages(images);

        if (!string.IsNullOrEmpty(description))
            SetDescription(description);
    }

    public static Result<ProductEntity, Error> Create(AddProductRequest request)
    {
        var productId = ProductId.Create(Guid.NewGuid());

        if (productId.IsFailure)
            return productId.Error;

        var price = Price.Create(request.Price, "IRR");

        if (price.IsFailure)
            return price.Error;

        return new ProductEntity(productId.Value, request.Title, request.Description, price.Value, request.Images);

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

    public override string ToString() =>
        $"Product [{Id}] - {Title} (${Price:F2})";
}