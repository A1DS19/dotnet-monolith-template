namespace DMT.Application.Common.Pagination;

public class PaginatedResponse<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    public PaginatedResponse() { }

    public PaginatedResponse(IEnumerable<T> data, int pageIndex, int pageSize, int totalCount)
    {
        Data = data;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        HasPreviousPage = pageIndex > 1;
        HasNextPage = pageIndex < TotalPages;
    }

    public static PaginatedResponse<T> Create(IEnumerable<T> data, int pageIndex, int pageSize, int totalCount)
    {
        return new PaginatedResponse<T>(data, pageIndex, pageSize, totalCount);
    }
}
