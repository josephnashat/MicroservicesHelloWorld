
namespace CatalogAPI.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler
    (IDocumentSession session//, ILogger<GetProductsQueryHandler> logger
    ) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        //logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);
        //var products = session.Query<Product>().ToListAsync(cancellationToken);
        var products = session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return Task.FromResult(new GetProductsResult(products.Result));
    }
}
