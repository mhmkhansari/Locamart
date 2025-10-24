namespace Locamart.Nava.Application.Contracts;

public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public Guid? NextCursor { get; }
    public bool HasNextPage { get; }

    public PagedResult(IReadOnlyList<T> items, Guid? nextCursor, bool hasNextPage)
    {
        Items = items;
        NextCursor = nextCursor;
        HasNextPage = hasNextPage;
    }
}