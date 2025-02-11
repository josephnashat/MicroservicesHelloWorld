namespace CatalogAPI.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    :ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);
internal class CreateProductCommandHandler (IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Business logic to create a product

        // Create a product entity
        var product = command.Adapt<Product>();
        // Save the product entity to the database
        session.Store(product);
        session.SaveChangesAsync(cancellationToken);
        // Return CreateProductResult with the ID of the created product
        return Task.FromResult(new CreateProductResult(product.Id));
    }
}
