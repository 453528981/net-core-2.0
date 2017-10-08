using Ayatta.Domain;
using Ayatta.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Cart
{
    public  class CartManager
    {
        private readonly DefaultStorage defaultStorage;
        private readonly IDistributedCache defaultCache;
        private readonly IDistributedCache cartCache;
        private readonly ILogger logger;

        public CartManager(DefaultStorage defaultStorage, IDistributedCache defaultCache, IDistributedCache cartCache, ILogger<CartManager> logger)
        {
            this.defaultStorage = defaultStorage;
            this.defaultCache = defaultCache;
            this.cartCache = cartCache;
            this.logger = logger;
        }

        public Cart GetCart(string guid, Platform platform, int mediaId = 0)
        {
            return new Cart(guid, platform, mediaId, defaultStorage, defaultCache, cartCache, logger);
        }

    }

}