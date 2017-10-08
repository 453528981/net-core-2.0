using Ayatta.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Ayatta.Web.Controllers
{
    public class GlobalController : BaseController
    {
        public GlobalController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<GlobalController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet("/global/catg/{id}")]
        public IActionResult Catg(int id = 0)
        {

            //using (var file = System.IO.File.OpenRead("F://project/vscode/cache/catg." + id + ".dat"))
            //{
            //    var data = Serializer.Deserialize<Ayatta.Model.Catg>(file);
            //    return Json(data);
            // }
            if (id < 0)
            {
                return NotFound();
            }
            var data = DefaultStorage.CatgMiniGet(id);
            return Json(data);
        }

        [HttpGet("/global/item/{id}")]
        public IActionResult Item(int id = 0)
        {
            if (id < 0)
            {
                return NotFound();
            }
            var data = DefaultStorage.ItemGet(id, true);
            return Json(data);
        }

        [HttpGet("/global/item/mini/{id}")]
        public IActionResult ItemMini(int id = 0)
        {
            if (id < 0)
            {
                return NotFound();
            }
            var data = DefaultStorage.ItemMiniGet(id);
            return Json(data);
        }

        [HttpGet("/global/weed")]
        public async Task<IActionResult> Weed(string dir = null, string lastFileName = null)
        {
            var userId = User.Id;
            var weedFs = WeedFs.Instance;
            var data = await weedFs.Explore("/" + userId + "/" + dir);
            data.Path = data.Path.Substring(1 + userId.ToString().Length); // Regex.Replace(data.Path, "^/\\d/", "/" + userId + "/");
            if (!string.IsNullOrEmpty(lastFileName))
            {
                var temp = new { data.LastFileName, data.ShouldDisplayLoadMore, data.Files };
                return Json(data.Files);
            }
            return Json(data);
        }

        /*

                [HttpGet("/global/props")]
                public IActionResult Props(int catgId = 0, int propId = 0)
                {
                    var props = LocalProdCache.GetProps(catgId);
                    var propVaues = LocalProdCache.GetPropValues(catgId);
                    if (propId > 0)
                    {
                        propVaues = LocalProdCache.GetPropValues(catgId).Where(x => x.PropId == propId).ToList();
                    }
                    var data = new { props, propVaues };
                    return Json(data);
                }
                */
    }
}