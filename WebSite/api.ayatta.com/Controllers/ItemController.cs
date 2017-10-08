using System;
using Ayatta.Api;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public class ItemController : BaseController
    {

        public ItemController( DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<ItemController> logger) : base(defaultStorage, defaultCache, logger)
        {

        }

        [HttpPost("item-get")]
        public ItemGetResponse ItemGet([FromBody]ItemGetRequest req)
        {
            var rep = new ItemGetResponse();

            rep.Data = DefaultStorage.ItemGet(req.Id, req.IncludeSkus);

            return rep;
        }
    }
}