namespace Basket.API.Basket.GetBasket;
public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart ShoppingCart);
public class GetBasketQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        // TODO: Get the basket from the repository
        var basket = await repository.GetBasketAsync(request.UserName, cancellationToken);
        return new GetBasketResult(basket!);
    }
}
