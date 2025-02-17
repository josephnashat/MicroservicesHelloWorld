using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data;

public class CachedBaskedRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
    public Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = cache.GetStringAsync(userName, cancellationToken).Result;
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return Task.FromResult(JsonSerializer.Deserialize<ShoppingCart>(cachedBasket));
        }
        var basket = repository.GetBasketAsync(userName, cancellationToken).Result;
        cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        return Task.FromResult(basket);
    }

    public Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        var shoppingCart = repository.StoreBasketAsync(basket, cancellationToken);
        cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
        }, cancellationToken);
        
        return shoppingCart;
    }
    public Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var res = repository.DeleteBasketAsync(userName, cancellationToken);
        cache.RemoveAsync(userName, cancellationToken);
        return res;
    }
}
