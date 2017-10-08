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
    [Route("ad")]
    public class AdController : BaseController
    {
        public AdController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<AdController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("hierarchy/{pid?}")]
        public IActionResult Hierarchy(int pid = 0)
        {
            var data = DefaultStorage.AdModuleList();
            var hierarchy = data.Select(x => new Node { Id = x.Id.ToString(), Text = x.Name, Pid = x.Pid.ToString() }).ToHierarchy(pid.ToString());
            return Json(hierarchy);
        }

        [HttpGet("module-data/{id}")]
        public IActionResult ModuleData(int id, bool current = false)
        {
            var data = DefaultStorage.AdModuleGet(id, true, current);
            return Json(data);
        }

        [HttpGet("module-detail/{id}")]
        public IActionResult ModuleDetail(int id, int pid = 0)
        {
            var data = new AdModule();
            if (id > 0)
            {
                data = DefaultStorage.AdModuleGet(id);
            }
            return PartialView(data);
        }

        [HttpPost("module-detail/{id}")]
        public async Task<IActionResult> ModuleDetail(int id, AdModule model, int pid = 0)
        {
            var now = DateTime.Now;

            var result = new Result();

            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入中文名称");
                return Json(result);
            }

            if (id > 0 && pid == 0)
            {
                var old = DefaultStorage.AdModuleGet(id);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {

                    result.Status = DefaultStorage.AdModuleUpdate(old);
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

            if (id == 0 && pid == 0)
            {
                result.Message = "参数错误";
                return Json(result);

            }
            model.Pid = pid;
            model.Extra = string.Empty;
            model.CreatedOn = now;
            model.ModifiedBy = string.Empty;
            model.ModifiedOn = now;

            var hs = GetParendIds(pid);
            model.Depth = hs.Count + 1;
            model.Path = string.Join(",", hs);

            var newId = DefaultStorage.AdModuleCreate(model);

            if (result.Status = newId > 0)
            {
                hs.Add(newId);//补全path
                var path = string.Join(",", hs);
                DefaultStorage.AdModulePathUpdate(newId, path);
                result.Success();
            }

            return Json(result);
        }

        private IList<int> GetParendIds(int id)
        {
            var dic = DefaultStorage.AdModuleIdDic();
            var hs = new HashSet<int>();
            hs.Add(id);
            var i = id;
            while (i > 0)
            {
                i = dic[i];
                if (i > 0)
                {
                    hs.Add(i);
                }
            }
            return hs.Reverse().ToList();
        }


        [HttpGet("{moduleId}/data")]
        public IActionResult AdItemList(int moduleId, int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            var model = new AdItemListModel();
            model.Keyword = keyword;
            model.ModuleId = moduleId;
            model.Module = DefaultStorage.AdModuleGet(moduleId);
            model.Items = DefaultStorage.AdItemPagedList(moduleId, page, size, keyword, status);
            return View(model);
        }

        [HttpGet("{moduleId}/item/{id?}")]
        public IActionResult AdItemDetail(int moduleId, int id)
        {
            var now = DateTime.Now;

            var model = new AdItemDetailModel();
            model.Item = new AdItem();
            model.Item.StartedOn = now;
            model.Item.StoppedOn = now.AddDays(5);
            if (id > 0)
            {
                model.Item = DefaultStorage.AdItemGet(id);
            }
            return View(model);
        }

        [HttpPost("{moduleId}/item/{id?}")]
        public async Task<IActionResult> AdItemDetail(int moduleId, int id, AdItem model)
        {
            var now = DateTime.Now;
            var result = new Result();

            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入名称");
                return Json(result);
            }
            if (model.Title.IsNullOrEmpty())
            {
                result.Error("请输入标题");
                return Json(result);
            }

            if (id > 0)
            {
                var old = DefaultStorage.AdItemGet(id);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {
                    result.Status = DefaultStorage.AdItemUpdate(old);
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

            model.ModuleId = moduleId;
            model.CreatedOn = now;
            model.ModifiedBy = string.Empty;
            model.ModifiedOn = now;
            var newId = DefaultStorage.AdItemCreate(model);

            if (result.Status = newId > 0)
            {
                result.Success();
            }

            return Json(result);
        }

    }

}