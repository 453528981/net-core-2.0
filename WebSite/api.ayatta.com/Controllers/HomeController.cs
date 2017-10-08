using Ayatta.Cart;
using Ayatta.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Xml.Linq;

namespace Ayatta.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<HomeController> logger) : base(defaultStorage, defaultCache, logger)
        {

        }

       
    }
    

    public class TestController : Controller
    {
        [HttpGet("test")]
        public string Index()
        {
            var xml = XElement.Load("https://wechatapi.tiantian.com/x.xml");
            var val = xml.Value;
            return val;
        }
    }
}