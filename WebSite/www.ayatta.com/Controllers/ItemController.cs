using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("item")]

    public class ItemController : BaseController
    {

        public ItemController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<ItemController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet("{id}.html")]
        public IActionResult Index(int id = 0, int skuId = 0)
        {
            var model = new ItemModel.Index();
            var item = DefaultStorage.ItemGet(id, true);
            if (item != null)
            {
                model.Item = item;
                if (skuId > 0)
                {
                    var sku = item.Skus.FirstOrDefault(x => x.Status == Prod.Status.Online && x.Id == skuId);
                    if (sku != null)
                    {
                        model.SelectdProps = sku.PropId.Split(';');
                    }
                }
            }
            return View(model);
        }

        [HttpGet("/{id}.json")]
        public IActionResult Mini(int id = 0)
        {
            if (id < 1)
            {
                return null;
            }
            var data = DefaultStorage.ItemMiniGet(id);
            return Json(data);
        }
    }
}