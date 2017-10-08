using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using Ayatta.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("article")]
    public class ArticleController : BaseController
    {
        public ArticleController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<ArticleController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet]
        public IActionResult ArticleList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            var model = new ArticleListModel();
            model.Keyword = keyword;
            model.Articles = DefaultStorage.ArticlePagedList(page, size, keyword, status);
            return View(model);
        }


        [HttpGet("detail/{id?}")]
        public IActionResult ArticleDetail(int id)
        {
            var now = DateTime.Now;

            var model = new ArticleDetailModel();
            model.Article = new Article();
            model.Article.StartedOn = now;
            model.Article.StoppedOn = now.AddDays(5);

            if (id > 0)
            {
                model.Article = DefaultStorage.ArticleGet(id);
            }
            return View(model);
        }

        [HttpPost("detail/{id?}")]
        public async Task<IActionResult> ArticleDetail(int id, Article model)
        {
            var now = DateTime.Now;

            var result = new Result();

            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入名称");
                return Json(result);
            }

            if (id > 0)
            {
                var old = DefaultStorage.ArticleGet(id);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {

                    result.Status = DefaultStorage.ArticleUpdate(old);
                    if (!result.Status)
                    {
                        result.Message = "更新失败";
                    }
                }
                else
                {
                    result.Message = "参数有误";
                }
                return Json(result);
            }

            model.Extra = string.Empty;
            model.UserId = User.Id;
            model.CreatedBy = User.Name;
            model.CreatedOn = now;
            model.ModifiedBy = User.Name;
            model.ModifiedOn = now;

            var newId = DefaultStorage.ArticleCreate(model);

            if (result.Status = newId > 0)
            {
                result.Success();
            }

            return Json(result);
        }

        [HttpPost("delete/{id}")]
        public IActionResult ArticleDel(int id)
        {
            var result = new Result();
            result.Status = DefaultStorage.ArticleDelete(id);
            return Json(result);
        }

    }
}