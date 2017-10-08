using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Extensions;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Authorize]
    public abstract class BaseController : AbstractController
    {
        public new Identity User
        {
            get { return base.User.Identity(); }
        }
        protected BaseController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<BaseController> logger) : base(defaultStorage, defaultCache, logger)
        {

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

        private IList<Catg.Tiny> CatgTinyList()
        {
            return DefaultStorage.CatgTinyList();
        }

        protected IList<Catg.Tiny> GetCatgs(int catgId)
        {
            var list = CatgTinyList();
            var ids = GetCatgIds(catgId);
            var array = ids.Reverse();
            var result = new List<Catg.Tiny>();
            foreach (var id in array)
            {
                var c = list.FirstOrDefault(x => x.Id == id);
                if (c.Id > 0)
                {
                    result.Add(c);
                }
            }

            return result;
        }

        protected IList<int> GetCatgIds(int catgId)
        {
            var list = CatgTinyList();
            Func<int, int> func = id => { return list.FirstOrDefault(x => x.Id == id).ParentId; };

            var result = new List<int>();
            result.Add(catgId);

            var i = func(catgId);
            while (i != 0)
            {
                result.Add(i);
                i = func(i);
            }
            return result;
        }

    }
}