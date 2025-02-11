
namespace CatalogAPI.Products.GetProductByCategory;

public record GetProductsByCategoryQuery(string Category): IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

internal class GetProductsByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductsByCategoryQueryHandler> logger) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsByCategoryQueryHandler.Handle called with {@Query}", query);
        var products = session.Query<Product>()
            .Where(p => p.Category.Contains(query.Category)).ToListAsync(cancellationToken);
        return Task.FromResult(new GetProductsByCategoryResult(products.Result));
    }
}
