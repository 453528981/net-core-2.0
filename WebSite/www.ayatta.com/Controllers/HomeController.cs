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

        [HttpGet("/")]
        public IActionResult Index()
        {
            //var ci = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            //ci.AddClaim(new Claim(ClaimTypes.Name, "3"));
            //ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, "13718414243"));

            //var cp = new ClaimsPrincipal(ci);

            //HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cp);
            return View();
        }

    }
}