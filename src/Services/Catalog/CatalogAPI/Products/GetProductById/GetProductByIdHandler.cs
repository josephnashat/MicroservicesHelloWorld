
namespace CatalogAPI.Products.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResult>;
public record class GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler(IDocumentSession session
    //, ILogger<GetProductByIdQueryHandler> logger
    ) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        //logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Query}", query);
        var product = session.LoadAsync<Product>(query.ProductId, cancellationToken);
        if (product.Result == null)
        {
            throw new ProductNotFoundException(query.ProductId);
        }
        return Task.FromResult(new GetProductByIdResult(product.Result));
    }
}
