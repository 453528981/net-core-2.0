
using System;

using Microsoft.Extensions.Options;

namespace Ayatta.Cart
{
    /// <summary>
    /// Configuration options for <see cref="CartOptions"/>.
    /// </summary>
    public class CartOptions : IOptions<CartOptions>
    {
        public TimeSpan Expire { get; set; } = new TimeSpan(2, 0, 0);

        //public RedisCacheOptions CacheOptions { get; set; }

        CartOptions IOptions<CartOptions>.Value => this;
    }
}