
using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using Ayatta.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("user")]
    public class UserController : BaseController
    {
        public UserController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<UserController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        public IActionResult UserList()
        {
            return View();
        }
    }
}