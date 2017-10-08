using System;
//using MediatR;
using System.Linq;
//using Ayatta.Event;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ayatta.Web.Controllers
{
    [Route("promotion")]
    public class PromotionController : BaseController
    {
       // private readonly IMediator mediator;
        public PromotionController( DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<PromotionController> logger) : base(defaultStorage, defaultCache, logger)
        {
            //this.mediator = mediator;
        }

        [HttpGet("activity-list")]
        public ActionResult ActivityList(int page = 1, int size = 30, bool global = false, string name = null, DateTime? startedOn = null, DateTime? stoppedOn = null, byte? status = null,bool json=false)
        {
            var now = DateTime.Now;
            var model = new PromotionModel.ActivityList();
            model.Name = name;
            model.Global = global;
            model.StartedOn = startedOn.ToString("yyyy-MM-dd HH:mm:ss", string.Empty);
            model.StoppedOn = stoppedOn.ToString("yyyy-MM-dd HH:mm:ss", string.Empty);

            model.Promotions = DefaultStorage.PromotionActivityPagedList(page, size, User.Id);
            //mediator.Publish(new PromotionChangedEvent());
            if (json)
            {
                return Json(model.Promotions);
            }
            return View(model);
        }


        [HttpGet("activity-detail/{id}")]
        public ActionResult ActivityDetail(int id = 0, bool global = false)
        {
            var now = DateTime.Now;

            var model = new PromotionModel.ActivityDetail();

            model.Global = global;
            model.Promotion = new Promotion.Activity();
            model.Promotion.StartedOn = now.AddDays(2);
            model.Promotion.StoppedOn = now.AddDays(20);

            if (id > 0)
            {
                var promotion = DefaultStorage.PromotionActivityGet(id);
                model.Promotion = promotion;
                model.Global = promotion.Global;
            }

            return View(model);
        }


        [HttpPost("activity-detail/{id}")]
        public async Task<IActionResult> Default(int id, bool global, Promotion.Activity model, Promotion.Activity.Rule[] rules)
        {

            var now = DateTime.Now;

            var result = new Result();
            if (id < 0)
            {
                result.Message = "参数错误";
                return Json(result);
            }
            if (model.Name.IsNullOrEmpty())
            {
                result.Error("请输入活动名称");
                return Json(result);
            }
            if (model.Name.Length > 100)
            {
                result.Error("活动名称限制为100个字符内");
                return Json(result);
            }
            if (model.Title.IsNullOrEmpty())
            {
                result.Error("请输入活动标题");
                return Json(result);
            }
            if (model.Title.Length > 100)
            {
                result.Error("活动标题限制为100个字符内");
                return Json(result);
            }
            if (model.StartedOn < now.AddHours(-1))
            {
                result.Error("活动开始时间不可小于当前时间");
                return Json(result);
            }
            if (model.StoppedOn < model.StartedOn)
            {
                result.Error("活动结束时间不可早于活动开始时间");
                return Json(result);
            }

            if (rules == null || rules.Length < 1)
            {
                result.Message = "请设置优惠规则";
                return Json(result);
            }

            rules = rules.Where(x => x.Threshold > 0 && x.Discount > 0).OrderByDescending(x => x.Threshold).ToArray();

            if (rules.Length < 1)
            {
                result.Message = "请设置优惠规则";
                return Json(result);
            }

            for (var i = 0; i < rules.Length; i++)
            {
                var urle = rules[i];

                if (i > 0)
                {  
                    var pre = rules[i - 1];
                    //if (model.Type == 0 && urle.Discount < pre.Discount)//保证当前层级优惠比上一级大
                    //{
                    //    result.Message = "优惠规则不合法";
                    //    return Json(result);
                    //}

                    //if (urle.SendGift && string.IsNullOrEmpty(urle.GiftData))
                    //{
                    //    if (string.IsNullOrEmpty(urle.GiftData))
                    //    {
                    //        result.Message = "赠品规则不合法";
                    //        return Json(result);
                    //    }
                    //}
                    //if (urle.SendCoupon)
                    //{
                    //    if (string.IsNullOrEmpty(urle.CouponData))
                    //    {
                    //        result.Message = "优惠券规则不合法";
                    //        return Json(result);
                    //    }
                    //}

                }
                //如果当前层级包邮 之后的层级都必须包邮 且不包邮地区都一样
                //if (urle.)
                //{
                //    for (var j = i; j < len; j++)
                //    {
                //        rules[j].FreightFreeSelected = true;
                //        rules[j].FreightFreeExclude = urle.FreightFreeExclude;
                //    }
                //}
            }

            if (id > 0)
            {
                var old = DefaultStorage.PromotionActivityGet(id);
                if (old == null)
                {
                    result.Message = "数据不存在";
                    return Json(result);
                }

                
                
                var status = await TryUpdateModelAsync(model);
                if (status)
                {
                    old.RuleData = JsonConvert.SerializeObject(rules);

                    result.Status = DefaultStorage.PromotionActivityUpdate(old);
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
            //检查时间段内是否有其它有效活动

            model.Global = global;
            model.FreightFreeExclude = string.Empty;
            model.ExternalUrl = string.Empty;
            model.RuleData = JsonConvert.SerializeObject(rules);
            model.SellerId = User.Id;
            model.SellerName = User.Name;
            model.Status = true;
            model.CreatedOn = now;
            model.ModifiedBy = "";
            model.ModifiedOn = now;
            var newId = DefaultStorage.PromotionActivityCreate(model);
            if (newId > 0)
            {
                result.Success();
            }

            return Json(result);
        }

        /*
       [Route("packages")]
       public ActionResult Packages(int page = 1, int size = 30)
       {
           var model = new PromotionModel.Packages();
           var identity = User.Identity.Identity();
           model.SelectedMenu = "promotion.packages";
           model.Promotions = DataCore.PromotionPackagePagedList(page, size);
           return View("packages", model);
       }
       */

    }
}