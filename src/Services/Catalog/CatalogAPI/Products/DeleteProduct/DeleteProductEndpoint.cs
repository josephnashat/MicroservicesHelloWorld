namespace CatalogAPI.Products.DeleteProduct;

//public record DeleteProductRequest(Guid ProductId);
public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id), cancellationToken);
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        })
            .WithName("DeleteProduct")
            .WithSummary("Delete a product.")
            .WithDescription("Delete a product.")
            .WithTags("Products")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
        ;
    }
}

