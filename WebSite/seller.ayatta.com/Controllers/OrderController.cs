using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<OrderController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet("/order-list")]
        public IActionResult OrderList()
        {
            var model = new OrderListModel();
            model.Orders = DefaultStorage.OrderPagedList();

            return View(model);
        }

        [HttpGet("/order-detail/{id}")]
        public IActionResult OrderDetail(string id)
        {
            var model = new OrderDetailModel();
            model.Order = DefaultStorage.OrderGet(id, User.Id, true, true);

            return View(model);
        }

        /// <summary>
        /// 订单备忘
        /// </summary>
        /// <param name="id">订单号</param>
        /// <returns></returns>
        [HttpGet("/order-memo/{id}")]
        public ActionResult OrderMemo(string id)
        {
            var identity = User;
            var model = DefaultStorage.OrderMemoGet(id, identity.Id, true);
            return PartialView(model);
        }

        /// <summary>
        /// 订单备忘
        /// </summary>
        /// <param name="param">订单号</param>
        /// <param name="flag">Flag</param>
        /// <param name="memo">备忘</param>
        /// <returns></returns>
        [HttpPost("/order-memo/{id}")]
        public ActionResult OrderMemo(string id, byte flag, string memo)
        {
            var result = new Result();
            if (!User)
            {
                result.Error("未登录或登录超时，请重新登录！");
                return Json(result);
            }
            if (memo.Length > 100)
            {
                result.Message = "备忘不能超过100个字！";
                return Json(result);
            }

            result.Status = DefaultStorage.OrderMemoUpdate(id, User.Id, true, flag, memo);

            return Json(result);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id">订单号</param>
        /// <returns></returns>
        [HttpGet("/order-cancel/{id}")]
        public ActionResult OrderCancel(string id)
        {
            var model = new Result<OrderStatus>();

            var status = DefaultStorage.OrderStatusGet(id, User.Id, true);

            model.Data = status;
            model.Status = true;

            return PartialView(model);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id">订单号</param>
        /// <param name="reason">取消原因</param>
        /// <returns></returns>
        [HttpPost("/order-cancel/{id}")]
        public ActionResult OrderCancel(string id, string reason)
        {
            var result = new Result();
            if (!User)
            {
                result.Error("未登录或登录超时，请重新登录！");
                return Json(result);
            }
            var status = DefaultStorage.OrderStatusGet(id, User.Id, true);

            if (status == OrderStatus.Pending || status == OrderStatus.WaitBuyerPay)
            {
                result.Status = DefaultStorage.OrderCancel(id, User.Id,true, 3, reason);
                if (result.Status)
                {
                    result.Status = true;
                }
                else
                {
                    result.Message = "取消订单失败！";
                }
            }
            else
            {
                result.Message = "该订单当前状态不可取消！";
            }
            return Json(result);
        }
    }

}


