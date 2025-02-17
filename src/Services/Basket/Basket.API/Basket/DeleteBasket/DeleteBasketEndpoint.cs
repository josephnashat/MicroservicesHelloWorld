namespace Basket.API.Basket.DeleteBasket;

//public record DeleteBasketRequest(string UserName);
public record DeleteBasketResponse(bool IsSuccess);
public class DeleteBasketEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string username, ISender sender, CancellationToken cancellationToken) =>
        {
            //var command = request.Adapt<DeleteBasketCommand>();
            var result = await sender.Send(new DeleteBasketCommand(username), cancellationToken);
            var response = result.Adapt<DeleteBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteBasket")
        .WithSummary("Delete Basket.")
        .WithDescription("Delete Basket.")
        .WithTags("Basket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

