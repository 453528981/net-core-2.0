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
    [Route("act")]
    public class ActController : BaseController
    {
        public ActController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<ActController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet("plan-list")]
        public IActionResult PlanList(int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            var model = new ActPlanListModel();
            model.Plans = DefaultStorage.ActPlanPagedList(page, size, keyword, status);
            return View(model);
        }

        [HttpGet("plan-detail/{id?}")]
        public IActionResult PlanDetail(int? id)
        {
            var now = DateTime.Now;
            var model = new ActPlanDetailModel();
            model.Plan = new ActPlan();
            model.Plan.OpendOn = now;
            model.Plan.ClosedOn = now.AddDays(5);
            model.Plan.StartedOn = model.Plan.ClosedOn.AddDays(1);
            model.Plan.StoppedOn = model.Plan.StartedOn.AddDays(5);

            if (id.HasValue && id.Value > 0)
            {
                model.Plan = DefaultStorage.ActPlanGet(id.Value);
            }
            return View(model);
        }

        [HttpPost("plan-detail/{id?}")]
        public async Task<IActionResult> PlanDetail(int? id, ActPlan model)
        {
            var now = DateTime.Now;
            var result = new Result<int>();

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
            if (model.OpendOn < now.AddDays(-1))
            {
                result.Error("报名开始时间不能小于当前时间");
                return Json(result);
            }
            if (model.ClosedOn < model.OpendOn)
            {
                result.Error("报名结束时间必需晚于报名开始时间");
                return Json(result);
            }
            if (model.StartedOn < model.OpendOn)
            {
                result.Error("活动开始时间必需晚于报名开始时间");//可持续报名
                return Json(result);
            }
            if (model.StoppedOn < model.StartedOn)
            {
                result.Error("活动结束时间必需晚于活动开始时间");
                return Json(result);
            }

            if (id.HasValue && id.Value > 0)
            {
                var old = DefaultStorage.ActPlanGet(id.Value);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {
                    result.Status = DefaultStorage.ActPlanUpdate(old);
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

            model.CreatedOn = now;
            model.ModifiedBy = string.Empty;
            model.ModifiedOn = now;

            var newId = DefaultStorage.ActPlanCreate(model);

            if (result.Status = newId > 0)
            {
                result.Data = newId;
                result.Success();
            }

            return Json(result);
        }

        [HttpPost("plan-delete/{id}")]
        public IActionResult PlanDel(int id)
        {
            var result = new Result();
            result.Status = DefaultStorage.ActPlanDelete(id);
            return Json(result);
        }

        [HttpGet("item-list/{planId}")]
        public IActionResult ActItemList(string planId, int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            var model = new ActItemListModel();
            model.PlanId = planId;
            model.Keyword = keyword;
            model.Items = DefaultStorage.ActItemPagedList(planId, page, size, keyword, status);
            return View(model);
        }

        [HttpGet("plan/{planId}/item/{id?}")]
        public IActionResult ActItemDetail(int planId, int id)
        {
            var now = DateTime.Now;
            var model = new ActItemDetailModel();
            model.Item = new ActItem();
            model.Item.StartedOn = now;
            model.Item.StoppedOn = now.AddDays(5);
            if (id > 0)
            {
                model.Item = DefaultStorage.ActItemGet(id);
            }
            return View(model);
        }

        [HttpPost("plan/{planId}/item/{id?}")]
        public async Task<IActionResult> ActItemDetail(int planId, int id, ActItem model)
        {
            var identity = User;

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
                var old = DefaultStorage.ActItemGet(id);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                var status = await TryUpdateModelAsync(old);
                if (status)
                {
                    result.Status = DefaultStorage.ActItemUpdate(old);
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

            model.SellerId = identity.Id;
            model.SellerName = identity.Name;
            model.CreatedOn = now;
            model.ModifiedBy = identity.Name;
            model.ModifiedOn = now;
            var newId = DefaultStorage.ActItemCreate(model);

            if (result.Status = newId > 0)
            {
                result.Success();
            }

            return Json(result);
        }

    }
}