using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Product.RequestModels;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Product;

public sealed class ProductEntity : Entity<ProductId>
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public Price Price { get; private set; }
    public int? Quantity { get; private set; }
    public List<Image> Images { get; } = [];
    public StoreId StoreId { get; init; }

    private ProductEntity() : base(default!) { }

    private ProductEntity(ProductId id, StoreId storeId, string title, int? quantity, string description, Price price, List<Image> images) : base(id)
    {
        StoreId = storeId;
        Title = title;
        Description = description;
        Price = price;
        Quantity = quantity;
        AddImages(images);

        if (!string.IsNullOrEmpty(description))
            SetDescription(description);
    }

    public static Result<ProductEntity, Error> Create(AddProductRequest request)
    {
        var productId = ProductId.Create(Guid.NewGuid());

        if (productId.IsFailure)
            return productId.Error;

        var storeId = StoreId.Create(request.StoreId);

        if (storeId.IsFailure)
            return storeId.Error;

        var price = Price.Create(request.Price, "IRR");

        if (price.IsFailure)
            return price.Error;

        return new ProductEntity(productId.Value, storeId.Value, request.Title, request.Quantity, request.Description, price.Value, request.Images);

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