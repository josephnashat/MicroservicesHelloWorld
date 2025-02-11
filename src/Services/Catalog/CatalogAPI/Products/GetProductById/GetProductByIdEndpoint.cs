
using CatalogAPI.Products.GetProducts;

namespace CatalogAPI.Products.GetProductById;

//public record GetProductByIdRequest(Guid ProductId);
public record GetProductByIdResponse(Product Product);
public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id), cancellationToken);
            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        })
       .WithName("GetProductById")
       .WithSummary("Get Product By Id.")
       .WithDescription("Get product by Id")
       .WithTags("Products")
       .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
       .ProducesProblem(StatusCodes.Status400BadRequest)
       .ProducesProblem(StatusCodes.Status404NotFound);

    }
}
