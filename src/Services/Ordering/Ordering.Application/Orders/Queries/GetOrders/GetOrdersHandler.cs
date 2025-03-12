
namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        int pageSize = query.PaginationRequest.PageSize;
        int pageIndex = query.PaginationRequest.PageIndex;
        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .OrderBy(o => o.OrderName)
            .Skip(pageIndex * pageSize)
            .Take(pageIndex)
            .ToListAsync(cancellationToken);

        return new GetOrdersResult(
            new PaginatedResult<OrderDto>(
                pageIndex: pageIndex,
                pageSize: pageSize,
                count: totalCount,
                data: orders.ToOrderDtoList()
                )
            );
    }
}
