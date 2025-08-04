using DMT.Application.Common.Pagination;

namespace DMT.Application.Common.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginatedRequest request)
    {
        return query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
    }

    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> enumerable, PaginatedRequest request)
    {
        return enumerable.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageIndex, int pageSize)
    {
        var request = new PaginatedRequest(pageIndex, pageSize);
        return query.Paginate(request);
    }

    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> enumerable, int pageIndex, int pageSize)
    {
        var request = new PaginatedRequest(pageIndex, pageSize);
        return enumerable.Paginate(request);
    }

    public static async Task<PaginatedResponse<T>> ToPaginatedResponseAsync<T>(
        this IQueryable<T> query,
        PaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        var totalCount = query.Count();
        var data = query.Paginate(request).ToList();

        return PaginatedResponse<T>.Create(data, request.PageIndex, request.PageSize, totalCount);
    }

    public static PaginatedResponse<T> ToPaginatedResponse<T>(
        this IEnumerable<T> enumerable,
        PaginatedRequest request)
    {
        var totalCount = enumerable.Count();
        var data = enumerable.Paginate(request);

        return PaginatedResponse<T>.Create(data, request.PageIndex, request.PageSize, totalCount);
    }
}
