using Ayatta.Nsq;
using Ayatta.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("help")]

    public class HelpController : BaseController
    {

        public HelpController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<HelpController> logger) : base(defaultStorage, defaultCache, logger)
        {

        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}