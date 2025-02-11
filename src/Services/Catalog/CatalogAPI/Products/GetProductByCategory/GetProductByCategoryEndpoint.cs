namespace CatalogAPI.Products.GetProductByCategory;

//public record GetProductsByCategoryRequest(string Category);
public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
public class GetProductByCategoryEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetProductsByCategoryQuery(category), cancellationToken);
            var response = result.Adapt<GetProductsByCategoryResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductByCategory")
        .WithSummary("Get Products By Category.")
        .WithDescription("Get products by category")
        .WithTags("Products")
        .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

