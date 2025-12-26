namespace Locamart.Nava.Application.Contracts.Dtos.ProductCategory;

public sealed class ProductCategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public Guid? ParentId { get; init; }
    public ProductCategoryStatus Status { get; init; }

    /// <summary>
    /// Depth in the tree (root = 0)
    /// </summary>
    public int Level { get; init; }

    /// <summary>
    /// Materialized path for ordering / breadcrumbs
    /// Example: {rootId}.{childId}.{leafId}
    /// </summary>
    public string Path { get; init; } = default!;
}
