namespace Locamart.Nava.Application.Contracts.Dtos.StoreCategory;

public class StoreCategoryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public Guid? ParentId { get; set; }

    public int Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTimeOffset? LastUpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }
}

