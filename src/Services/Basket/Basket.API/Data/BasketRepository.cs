namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var basket = session.LoadAsync<ShoppingCart>(userName, cancellationToken);
        return basket.Result is null ? throw new BasketNotFoundException(userName) : basket;
    }
     
    public Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        session.Store<ShoppingCart>(basket);
        session.SaveChangesAsync(cancellationToken);
        return Task.FromResult(basket);
    }
    public Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        session.Delete<ShoppingCart>(userName);
        session.SaveChangesAsync(cancellationToken);
        return Task.FromResult(true);
    }

}
