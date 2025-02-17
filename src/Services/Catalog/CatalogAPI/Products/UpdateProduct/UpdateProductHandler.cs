﻿
namespace CatalogAPI.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required"); ;
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").Length(2, 150).WithMessage("Name must be between 2 and 150");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than Zero");
    }
}
internal class UpdateProductCommandHandler
    (IDocumentSession session//, ILogger<UpdateProductCommandHandler> logger
    ) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        //logger.LogInformation("Updating product {Id}", command.Id);
        var product = session.LoadAsync<Product>(command.Id, cancellationToken).Result ?? throw new ProductNotFoundException(command.Id);
        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        session.Update(product);
        session.SaveChangesAsync(cancellationToken);

        return Task.FromResult(new UpdateProductResult(true));
    }
}
