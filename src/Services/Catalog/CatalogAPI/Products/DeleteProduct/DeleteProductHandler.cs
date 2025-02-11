
namespace CatalogAPI.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);
internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting product {Id}", command.Id);
        var product = session.LoadAsync<Product>(command.Id, cancellationToken).Result ?? throw new ProductNotFoundException(command.Id);
        session.Delete(product);
        session.SaveChangesAsync(cancellationToken);

        return Task.FromResult(new DeleteProductResult(true));
    }
}
