using Ayatta.Nsq;
using Ayatta.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("about")]

    public class AboutController : BaseController
    {

        public AboutController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<AboutController> logger) : base(defaultStorage, defaultCache, logger)
        {

        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}