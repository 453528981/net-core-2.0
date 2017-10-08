using Ayatta.Storage;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.RegularExpressions;
using Ayatta.Domain;
using System.Linq;
using Ayatta.Extension;

namespace Ayatta.Web.Controllers
{
    [Route("global")]
    public class GlobalController : BaseController
    {
        public GlobalController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<GlobalController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet("catg/{id}")]
        public IActionResult Catg(int id = 0)
        {
            if (id > 0)
            {
                var data = DefaultStorage.CatgMiniGet(id);
                return Json(data);
            }
            return NotFound();
        }

        [HttpGet("item/{id}")]
        public IActionResult Item(int id = 0)
        {
            if (id > 0)
            {
                var data = DefaultStorage.ItemGet(id, true);
                return Json(data);
            }
            return NotFound();
        }

        [HttpGet("item/mini/{id}")]
        public IActionResult ItemMini(int id = 0)
        {
            if (id > 0)
            {
                var data = DefaultStorage.ItemMiniGet(id);
                return Json(data);
            }
            return NotFound();
        }

        [HttpGet("catg/parents/{catgId}")]
        public IActionResult CatgParents(int catgId = 0)
        {
            if (catgId > 0)
            {
                var data = GetCatgs(catgId);
                return Json(data);
            }
            return NotFound();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="keyword"></param>
        /// <param name="catgId"></param>
        /// <param name="brandId"></param>
        /// <param name="include"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        [HttpGet("items")]

        public IActionResult Items(int page = 1, int size = 20, string keyword = null, int? catgId = null, int? brandId = null, Prod.Status[] status = null, string include = null, string exclude = null)
        {
            int[] inc = null;
            if (!string.IsNullOrEmpty(include))
            {
                inc = include.Split(',').Where(x => Regex.IsMatch(x, "^\\d+$")).Select(x => x.AsInt()).ToArray();
            }
            int[] exc = null;
            if (!string.IsNullOrEmpty(exclude))
            {
                exc = exclude.Split(',').Where(x => Regex.IsMatch(x, "^\\d+$")).Select(x => x.AsInt()).ToArray();
            }
            var data = DefaultStorage.ItemTinyList(page, size, keyword, catgId, brandId, User.Id, status, inc, exc);
            var o = new { data = data, totalPage = data.TotalPages, totalRecord = data.TotalRecords };
            return Json(o);
        }

        [HttpGet("weed")]
        public async Task<IActionResult> Weed(string dir = null, string lastFileName = null)
        {
            var userId = User.Id;
            var weedFs = WeedFs.Instance;
            var data = await weedFs.Explore("/" + userId + "/"+dir);
            data.Path = Regex.Replace(data.Path, "^/\\d/", "/" + userId + "/");
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