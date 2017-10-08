using System.Linq;
using Ayatta.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public class SharedController : AbstractController
    {
        public SharedController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<SharedController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [Route("/shared/footer")]
        public IActionResult Footer()
        {
            return PartialView();
        }

        [Route("/shared/suggest")]

        public IActionResult Suggest(string q)
        {
            var list = new List<string>();
            list.Add("aa");
            list.Add("ab");
            list.Add("cd");
            list.Add("ad");
            list.Add("dd");
            var data = list.Select((x, i) => new { Id = i, Value = x });
            return Json(data);
        }
    }
}