using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Extensions;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
   
    public abstract class BaseController : AbstractController
    {
        public new Identity User
        {
            get { return base.User.Identity(); }
        }
        protected BaseController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<BaseController> logger) : base(defaultStorage, defaultCache, logger)
        {

        }
             

    }
}