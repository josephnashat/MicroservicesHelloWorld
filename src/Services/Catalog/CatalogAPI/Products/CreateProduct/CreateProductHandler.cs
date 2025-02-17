namespace CatalogAPI.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required"); ;
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required"); ;
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than Zero"); ;
    }
}
internal class CreateProductCommandHandler
    (IDocumentSession session
    //, ILogger<CreateProductCommandHandler> logger
    //, IValidator<CreateProductCommand> validator
    )
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Business logic to create a product
        //logger.LogInformation("Creating a new product with name {Name}", command.Name);

        //// Validate the command
        //var validationResult = validator.ValidateAsync(command, cancellationToken).Result;
        //var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
        //if (errors.Any())
        //{
        //    throw new ValidationException(errors.FirstOrDefault());
        //}



        // Create a product entity
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // Save the product entity to the database
        session.Store(product);
        session.SaveChangesAsync(cancellationToken);
        // Return CreateProductResult with the ID of the created product
        return Task.FromResult(new CreateProductResult(product.Id));
    }
}
