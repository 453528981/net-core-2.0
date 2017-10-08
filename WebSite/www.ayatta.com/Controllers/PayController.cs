using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.OnlinePay;
using Ayatta.Web.Models;
using Ayatta.Web.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("pay")]
    public class PayController : BaseController
    {
        public PayController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<PayController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        /*
        [HttpGet("for/{id}")]
        public IActionResult Pay(string id)
        {
            var payment = DefaultStorage.PaymentGet(id);
            return Json(payment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">支付单Id</param>
        /// <param name="platformId">支付平台Id</param>
        /// <param name="bankId">支付银行Id</param>
        /// <returns></returns>
        [HttpPost("for/{id}")]
        public IActionResult Pay(string id, int platformId = 2, int bankId = 0)
        {
            var payment = DefaultStorage.PaymentGet(id);
            var onlinePay = GetOnlinePay(platformId);
            if (onlinePay != null)
            {
                payment.Amount = 0.01m;//测试
                var url = onlinePay.Pay(payment);
                if (platformId == 3)
                {
                    //RenderQRCode(url);
                }
                return Redirect(url);
            }
            return View();
        }
        */

        /// <summary>
        /// 用户在支付平台支付完成后支付平台会重定向到该地址
        /// </summary>
        /// <param name="platformId">支付平台Id</param>
        /// <returns></returns>
        [HttpGet("callback/{platformId}")]
        public IActionResult Callback(int platformId = 2)
        {
            var onlinePay = GetOnlinePay(platformId);
            if (onlinePay == null) return StatusCode(404);
            var param = new PaymentParam(Request.Query);
            var result = onlinePay.HandleNotify(param);
            if (!result)
            {
                Logger.LogError(1, "{onlinePay} Callback 处理失败 支付单({payId}) 支付平台交易号{payNo} {message}", onlinePay.Name, result.Data.PayId, result.Data.PayNo, result.Message);
            }
            return Redirect("/pay/result/" + result.Data.PayId);//跳转到结果页面统一处理
        }

        /// <summary>
        /// 支付平台异步通知
        /// </summary>
        /// <param name="platformId">支付平台Id</param>
        /// <returns></returns>
        [Route("notify/{platformId}")]
        public IActionResult Notify(int platformId = 2)
        {
            var onlinePay = GetOnlinePay(platformId);
            if (onlinePay == null) return StatusCode(404);
            var param = new PaymentParam(Request.Query);
            var result = onlinePay.HandleNotify(param);
            if (!result)
            {
                Logger.LogError(1, "{onlinePay} Notify 处理失败 支付单({payId}) 支付平台交易号{payNo} {message}", onlinePay.Name, result.Data.PayId, result.Data.PayNo, result.Message);
            }
            return Content(result.Data.Output);
        }

        /// <summary>
        /// 支付结果
        /// </summary>
        /// <param name="id">支付单号</param>
        /// <returns></returns>
        [HttpGet("result/{id}")]
        public ActionResult Result(string id)
        {
            return View();
        }

        #region 订单支付
        /// <summary>
        /// 待支付订单
        /// </summary>
        /// <param name="id">订单Id</param>
        /// <returns></returns>
        [HttpGet("order/{id}")]
        public IActionResult Order(string id)
        {
            var identity = User;
            if (!identity)
            {
                return Content("登录");
            }

            var status = DefaultStorage.OrderStatusGet(id, identity.Id, false);
            if (status != OrderStatus.WaitBuyerPay)
            {
                return Redirect("http://localhost:39272/order/detail/" + id);
            }
            var model = new PayOrder();
            model.Account = new Account();
            var order = DefaultStorage.OrderGet(id, identity.Id, false, true);

            if (order.PaymentType.IsOnlinePay() && order.Status == OrderStatus.WaitBuyerPay && order.Paid < order.Total)
            {
                var platforms = PaymentPlatformList();
                //var tempEBanks = PaymentEBankList();
                //var eBanks = new List<PaymentEBank>();

                //foreach (var platform in platforms)
                //{
                //    var p = platform;
                //    var x = eBanks.Select(o => o.BankId);
                //    var temp = tempEBanks.Where(o => o.PlatformId == p.Id && o.Status && !x.Contains(o.BankId));
                //    eBanks.AddRange(temp);
                //}


                model.Order = order;
                model.Platforms = platforms;
                //model.Banks = eBanks;
                return View(model);
            }
            return Redirect("http://localhost:39272/order/detail/" + id);
        }

        /// <summary>
        /// 待支付订单
        /// </summary>
        /// <param name="id">订单Id</param>
        /// <param name="platformId">支付平台Id</param>
        /// <param name="bankId">支付银行Id</param>
        /// <returns></returns>
        [HttpPost("order/{id}")]
        public IActionResult Order(string id, int platformId, int bankId = 0)
        {
            //可处理多次提交支付情况
            var identity = User;
            if (!identity)
            {
                //未登录
            }
            //再次检查订单状态
            var status = DefaultStorage.OrderStatusGet(id, identity.Id, false);
            if (status != OrderStatus.WaitBuyerPay)
            {
                return Redirect("http://localhost:39272/order/detail/" + id);
            }

            var order = DefaultStorage.OrderMiniGet(id, identity.Id);

            if (order.PaymentType.IsOnlinePay() && order.Status == OrderStatus.WaitBuyerPay && order.Paid < order.Total)
            {
                var onlinePay = GetOnlinePay(platformId);
                if (onlinePay != null)
                {
                    var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    var payment = order.ToPayment(platformId, ipAddress);//支付宝 微信需ip
                    if (bankId > 0)
                    {
                        var paymentBanks = DefaultStorage.PaymentBankList(platformId);
                        var paymentBank = paymentBanks.FirstOrDefault(o => o.Id == bankId);
                        if (paymentBank != null)
                        {
                            //如果用户选择了具体的银行则填充Payment相关银行信息
                            payment.BankId = paymentBank.Id;
                            payment.BankCode = paymentBank.Code;
                            payment.BankName = paymentBank.Bank.Name;
                        }
                    }

                    var created = DefaultStorage.PaymentCreate(payment);
                    if (created)
                    {
                        var url = onlinePay.Pay(payment);
                        if (string.IsNullOrEmpty(url))
                        {
                            return Content("创建支付平台信息失败");
                        }
                        if (platformId == 3)
                        {
                            //RenderQRCode(url);
                        }
                        return Redirect(url);//跳转到第三方支付平台进行支付
                    }
                    return Content("创建支付信息失败");
                }
                return Content("参数错误(OnlinePay)");
            }

            return Redirect("http://i.ayatta.com/order/detail/" + order.Id);
        }

        #endregion

        #region protected
        protected IOnlinePay GetOnlinePay(int platformId)
        {
            var platforms = DefaultStorage.PaymentPlatformList();
            var platform = platforms.FirstOrDefault(x => x.Id == platformId);

            if (platform == null) return null;

            var onlinePay = OnlinePayFactory.Create(platform);
            if (onlinePay != null)
            {
                onlinePay.Notified += Notified;
            }
            return onlinePay;
        }

        /// <summary>
        /// 支付平台通知验证成功后触发
        /// 更新付款单及订单状态等
        /// </summary>
        /// <param name="notification">支付平台通知</param>
        /// <returns></returns>
        private bool Notified(Notification notification)
        {
            try
            {
                var now = DateTime.Now;
                var payment = DefaultStorage.PaymentGet(notification.PayId);
                if (payment == null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        Logger.LogWarning(1, "支付单({0})未找到", notification.PayId);

                        var note = new PaymentNote();
                        note.PayId = notification.PayId;
                        note.PayNo = notification.PayNo;
                        note.UserId = 0;
                        note.Subject = "支付单未找到";
                        note.Message = string.Empty;
                        note.RawData = notification.Input;
                        note.Extra = string.Empty;
                        note.CreatedBy = "sys";
                        note.CreatedOn = now;

                        PaymentNoteCreate(note);
                    });
                    return false;
                }
                if (payment.Status)
                {
                    return true;//付款单已支付成功 直接返回true  (支付平台可能会多次通知)
                }

                var status = DefaultStorage.PaymentStatusUpdate(notification.PayId, notification.PayNo, true);//更新支付单状态

                if (status)
                {
                    //异步处理支付成功后的相关逻辑
                    Task.Factory.StartNew(() => Async(payment));
                }
                else
                {
                    Task.Factory.StartNew(() =>
                    {
                        Logger.LogWarning(1, "更新支付单({0})状态失败", notification.PayId);

                        var note = new PaymentNote();
                        note.PayId = notification.PayId;
                        note.PayNo = notification.PayNo;
                        note.UserId = payment.UserId;
                        note.Subject = "更新支付单状态失败";
                        note.Message = string.Empty;
                        note.RawData = notification.Input;
                        note.Extra = string.Empty;
                        note.CreatedBy = "sys";
                        note.CreatedOn = now;

                        PaymentNoteCreate(note);
                    });
                }

                return status;
            }
            catch (Exception e)
            {
                Logger.LogError(1, e, "处理支付平台通知发生异常");

                return false;
            }
        }

        private void PaymentNoteCreate(PaymentNote note)
        {
            var status = DefaultStorage.PaymentNoteCreate(note) > 0;

            if (!status)
            {
                Logger.LogError(1, "生成PaymentNote失败 支付单({0})", note.PayId);
            }
        }

        /// <summary>
        /// 处理订单 支持多次支付/合并支付
        /// </summary>
        /// <param name="payment"></param>
        private void Async(Payment payment)
        {
            var now = DateTime.Now;

            if (payment.Status && payment.Type == 0)
            {
                var fail = 0m;//更新失败
                var remain = payment.Amount;//单独或合并支付总金额

                var oids = payment.RelatedId.Split(',');//可能为多个订单号
                foreach (var oid in oids)
                {
                    if (remain <= 0)
                    {
                        break;
                    }
                    var order = DefaultStorage.OrderMiniGet(oid, payment.UserId); //获取订单
                    if (order != null)
                    {
                        //订单为在线支付 状态为待付款
                        if (order.PaymentType.IsOnlinePay() && order.Status == OrderStatus.WaitBuyerPay)
                        {
                            var status = false;//更新订单结果
                            var amount = order.Unpaid;//待支付金额
                            if (remain - amount >= 0)
                            {
                                //付清(付清后如果是在线支付订单则会更新订单状态为已付款待发货并更新订单有效期)
                                status = DefaultStorage.OrderPaid(oid, amount, true); //更新订单已付金额及状态
                                if (status)
                                {
                                    remain -= amount;
                                }
                                else
                                {
                                    fail += amount;
                                }
                            }
                            else
                            {
                                //剩余金额不足 需多次支付
                                status = DefaultStorage.OrderPaid(oid, remain, false);
                                if (status)
                                {
                                    remain = 0;
                                }
                                else
                                {
                                    fail += remain;
                                }
                            }

                            if (!status)
                            {
                                Logger.LogWarning(2, "更新订单({orderId})已付金额及状态失败", oid);

                                var note = new OrderNote();
                                note.Type = 1;
                                note.UserId = payment.UserId;
                                note.OrderId = oid;
                                note.Subject = "更新订单已付金额/状态失败";
                                note.Message = string.Format("支付单({0})更新订单({1})已付金额/状态失败", payment.Id, oid);
                                note.Extra = (remain - amount >= 0 ? amount : remain).ToString("F2");
                                note.CreatedBy = "sys";
                                note.CreatedOn = now;

                                if (DefaultStorage.OrderNoteCreate(note) == 0)
                                {
                                    Logger.LogError(2, "生成OrderNote失败 支付单({paymentId}) 订单({orderId}", payment.Id, oid);
                                }

                            }

                        }
                    }
                    else
                    {
                        Logger.LogWarning(1, "支付单({paymentId})未匹配到订单({orderId})", payment.Id, oid);

                        var note = new PaymentNote();
                        note.PayId = payment.Id;
                        note.PayNo = payment.No;
                        note.UserId = payment.UserId;
                        note.Subject = "支付单未匹配到订单";
                        note.Message = string.Format("支付单({0})未匹配到订单({1})", payment.Id, oid);
                        note.RawData = string.Empty;
                        note.Extra = oid;
                        note.CreatedBy = "sys";
                        note.CreatedOn = now;

                        if (DefaultStorage.PaymentNoteCreate(note) == 0)
                        {
                            Logger.LogError(1, "生成PaymentNote失败 支付单({paymentId}) 订单({orderId})", payment.Id, oid);
                        }
                    }
                }

                if (remain - fail > 0)
                {
                    //用户在支付平台支付成功 但未能及时收到通知或成功处理 造成用户多次支付 需把用户多支付的金额 退到用户钱包


                }
            }
        }

        #endregion
    }
}