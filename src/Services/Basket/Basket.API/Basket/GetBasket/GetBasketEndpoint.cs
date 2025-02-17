
namespace Basket.API.Basket.GetBasket;

//public record GetBasketRequest(string UserName);
public record GetBasketResponse(ShoppingCart ShoppingCart);
public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{username}", async (string username, ISender sender, CancellationToken cancellationToken) =>
        {
            //var query = request.Adapt<GetProductsQuery>();
            var result = await sender.Send(new GetBasketQuery(username), cancellationToken);
            var response = result.Adapt<GetBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBasketByUsername")
        .WithSummary("Get Basket By Username.")
        .WithDescription("Get Basket By Username.")
        .WithTags("Basket")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
