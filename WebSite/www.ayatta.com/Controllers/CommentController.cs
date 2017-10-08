using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Models;
using Ayatta.Storage.Param;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("comment")]

    public class CommentController : BaseController
    {
        public CommentController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<CommentController> logger) : base(defaultStorage, defaultCache, logger)
        {

        }

        [HttpGet("{id}.html")]
        public IActionResult Index(int id, int skuId = 0, int type = 0, int page = 1, int size = 20)
        {
            var model = new CommentIndexModel();
            model.Item = DefaultStorage.ItemMiniGet(id);
            model.Comment = DefaultStorage.ItemCommentGet(id);
            model.Comments = DefaultStorage.CommentPagedList(id, skuId, type, page, size);
            return View(model);
        }

        [HttpPost("create")]
        public IActionResult Create(int id)
        {
            var result = new Result();
            var exist = DefaultStorage.ItemCommentExist(id);
            if (!exist)
            {
                DefaultStorage.ItemCommentCreate(id);
            }
            return Json(result);
        }

    }
}
