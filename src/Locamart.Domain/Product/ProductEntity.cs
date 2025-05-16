using CSharpFunctionalExtensions;
using Locamart.Domain.Product.RequestModels;
using Locamart.Domain.Product.ValueObjects;
using Locamart.Shared;

namespace Locamart.Domain.Product;

public sealed class ProductEntity : Shared.Entity<ProductId>
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public List<string> Images { get; } = [];

    private ProductEntity(ProductId id, string title, string description, decimal price, List<string> images) : base(id)
    {
        Title = title;
        Description = description;
        SetTitle(title);
        SetPrice(price);
        AddImages(images);

        if (!string.IsNullOrEmpty(description))
            SetDescription(description);
    }

    public static Result<ProductEntity, Error> Create(AddProductRequest request)
    {
        return new ProductEntity(ProductId.Create(Guid.NewGuid()), request.Title, request.Description, request.Price, request.Images);
    }

    public UnitResult<Error> SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Error.Create("product_title_cannot_be_empty", "Product title cannot be empty");

        Title = title;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> SetDescription(string description)
    {
        Description = description;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> SetPrice(decimal price)
    {
        if (price < 0)
            return Error.Create("price_not_negative", "Price cannot be negative.");

        Price = price;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddImages(IEnumerable<string> images)
    {
        Images.AddRange(images);
        return UnitResult.Success<Error>();
    }

    public override string ToString() =>
        $"Product [{Id}] - {Title} (${Price:F2})";
}