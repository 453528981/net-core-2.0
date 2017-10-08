using Ayatta.Storage;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
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
            var ci = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            ci.AddClaim(new Claim(ClaimTypes.Name, "1"));
            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, "420303865@qq.com"));

            var cp = new ClaimsPrincipal(ci);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cp);
            return View();
        }

        

    }


}