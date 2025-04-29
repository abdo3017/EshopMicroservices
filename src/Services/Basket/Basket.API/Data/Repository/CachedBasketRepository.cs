
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data.Repository
{
    public class CachedBasketRepository(IBasketRepository basketRepository,IDistributedCache cache) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await basketRepository.DeleteBasket(userName, cancellationToken);
            await cache.RemoveAsync(userName, cancellationToken);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedbasket = await cache.GetStringAsync(userName, cancellationToken);
            if (cachedbasket != null)
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedbasket)!;
            }

            var basket = await basketRepository.GetBasket(userName,cancellationToken);
            await cache.SetStringAsync(userName,JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await basketRepository.StoreBasket(basket, cancellationToken);
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }
    }
}
