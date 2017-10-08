using Ayatta.Storage;
using Ayatta.Web.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public abstract class BaseController : AbstractController
    {
        public new Identity User
        {
            get { return base.User.Identity(); }
        }
        public BaseController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<BaseController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }
    }
}