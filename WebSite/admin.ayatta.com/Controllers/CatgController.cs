using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public class CatgController : BaseController
    {
        public CatgController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<CatgController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet("/catg")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/catg/children/{catgId}")]
        public IActionResult Children(int catgId)
        {
            var data = DefaultStorage.CatgChildren(catgId);
            return PartialView(data);
        }

        [HttpGet("/catg/nodes")]
        public IActionResult Nodes()
        {
            var data = DefaultStorage.CatgList();
            var nodes = data.Select(x => new Node { Id = x.Id.ToString(), Text = x.Name, Pid = x.ParentId.ToString(), IsParent = x.IsParent }).ToHierarchy("0");
            return Json(nodes);
        }

        [HttpGet("/catg/props/{catgId}")]

        public IActionResult Props(int catgId)
        {
            var data = DefaultStorage.CatgPropList(catgId);
            return PartialView(data);
        }

        [HttpGet("/catg/propvalues/{catgId}/{propId}")]

        public IActionResult PropValues(int catgId, int propId)
        {
            var data = DefaultStorage.CatgPropValueList(catgId, propId);
            return PartialView(data);
        }
    }

}