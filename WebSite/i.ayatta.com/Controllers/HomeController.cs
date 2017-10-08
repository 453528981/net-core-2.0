using Ayatta.Storage;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ayatta.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<HomeController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Content("test");
        }
       

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Address()
        {
            return View();
        }


    }


}