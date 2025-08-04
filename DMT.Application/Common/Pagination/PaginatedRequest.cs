namespace DMT.Application.Common.Pagination;

public class PaginatedRequest
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public PaginatedRequest() { }

    public PaginatedRequest(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex > 0 ? pageIndex : 1;
        PageSize = pageSize > 0 ? pageSize : 10;
    }
}
