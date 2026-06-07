namespace Application.Results;

public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int Page { get; }
    public int PageSize { get; }
    private int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    private bool HasNextPage => Page < TotalPages;
    private bool HasPreviousPage => Page > 1;

    public object Transactions { get; }
    public object Page1 { get; }
    public object PageSize1 { get; }

    private PagedResult(IReadOnlyList<T> items, int totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

    public PagedResult(object transactions, object page, object pageSize, int totalCount)
    {
        Transactions = transactions;
        Page1 = page;
        PageSize1 = pageSize;
        TotalCount = totalCount;
    }

    public static PagedResult<T> Ok(IReadOnlyList<T> items, int totalCount, int page, int pageSize)
        => new(items, totalCount, page, pageSize);
}
