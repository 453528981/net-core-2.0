using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.OnlinePay;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("api")]
    public abstract class BaseController
    {
        //protected CartManager CartManager { get; }
        protected IDistributedCache DefaultCache { get; }
        protected DefaultStorage DefaultStorage { get; }
        protected ILogger Logger { get; }
        protected BaseController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger logger)
        {
            //if (cartManager == null)
            //{
            //    throw new ArgumentNullException(nameof(cartManager));
            //}
            if (defaultStorage == null)
            {
                throw new ArgumentNullException(nameof(defaultStorage));
            }
            //CartManager = cartManager;
            DefaultStorage = defaultStorage;
            DefaultCache = defaultCache;
            Logger = logger;
        }


        /// <summary>
        /// 行政区列表(从缓存里取 有效7天)
        /// </summary>
        /// <returns></returns>
        protected IList<Region> RegionList()
        {
            var key = "base-region";
            return DefaultCache.Put(key, () => DefaultStorage.RegionList(), DateTime.Now.AddDays(7));
        }

        protected IOnlinePay GetOnlinePay(int platformId)
        {
            var platforms = DefaultStorage.PaymentPlatformList();
            var platform = platforms.FirstOrDefault(x => x.Id == platformId);

            if (platform == null) return null;

            return OnlinePayFactory.Create(platform);
        }
    }
}