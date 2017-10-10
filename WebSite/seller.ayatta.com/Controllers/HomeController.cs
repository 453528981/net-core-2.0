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

        [HttpGet("/test")]
        public IActionResult Test(int id = 1)
        {           
            return Content("test");
        }

        [HttpGet("/sign-in")]
        public IActionResult Index(int id=1)
        {
            var user = DefaultStorage.UserGet(id);

            var ci = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            ci.AddClaim(new Claim("id", user.Id.ToString()));
            ci.AddClaim(new Claim("guid", user.Guid));

            var cp = new ClaimsPrincipal(ci);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cp);
            return View();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="redirect">退出后跳转Url</param>
        /// <returns></returns>
        [HttpGet("/sign-out")]
        public IActionResult SignOut(string redirect = null)
        {

            base.SignOut();
            return Redirect(!string.IsNullOrEmpty(redirect) ? redirect : "/");
        }


    }


}