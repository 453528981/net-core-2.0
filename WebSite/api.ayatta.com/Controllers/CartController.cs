using Ayatta.Api;
using Ayatta.Cart;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.OnlinePay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class CartController : BaseController
    {
        private readonly CartManager cartManager;

        private readonly Platform plateform = Platform.App;
        public CartController(CartManager cartManager, DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<CartController> logger) : base(defaultStorage, defaultCache, logger)
        {
            this.cartManager = cartManager;
        }

        #region 获取
        /// <summary>
        /// 购物车获取
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-get")]
        public CartGetResponse CartGet([FromBody]CartGetRequest req)
        {

            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartGetResponse();

            var temp = cart.GetData();
            if (temp)
            {
                rep.Data = temp.Data;
            }

            return rep;
        }
        #endregion

        #region 操作
        /// <summary>
        /// 购物车商品操作
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-opt")]
        public CartOptResponse CartOpt([FromBody]CartOptRequest req)
        {
            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartOptResponse();

            var temp = cart.ProductOpt(req.Opt, req.ItemId, req.SkuId, req.Quantity);
            if (temp)
            {
                rep.Data = temp.Data;
            }
            else
            {
                rep.Error(temp.Message);
            }

            return rep;
        }
        #endregion

        #region 选择
        /// <summary>
        /// 购物车商品选择
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-select")]
        public CartSelectResponse CartSelect([FromBody]CartSelectRequest req)
        {
            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartSelectResponse();

            var temp = cart.Select(req.Select, req.Param, req.Selected);
            if (temp)
            {
                rep.Data = temp.Data;
            }
            else
            {
                rep.Error(temp.Message);
            }
            return rep;
        }

        #endregion

        #region 清除
        /// <summary>
        /// 购物车商品清除
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-clean")]
        public CartCleanResponse CartClean([FromBody]CartCleanRequest req)
        {
            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartCleanResponse();
            if (req.All)
            {
                var status = cart.Empty();
                if (!status)
                {
                    rep.Error("清空购物车失败");
                }
                return rep;
            }
            var temp = cart.Clean(req.Skus, req.Items, req.Packages);
            if (temp)
            {
                rep.Data = temp.Data;
            }
            else
            {
                rep.Error(temp.Message);
            }
            return rep;
        }

        #endregion

        #region 确认
        /// <summary>
        /// 购物车确认
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-confirm")]
        public CartConfirmResponse CartConfirm([FromBody]CartConfirmRequest req)
        {
            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartConfirmResponse();

            //var temp = cart.Confirm(req.Skus, req.Items, req.Packages, req.AddressId);
            //if (temp)
            //{
            //    rep.Data = temp.Data;
            //}
            //else
            //{
            //    rep.Error(temp.Message);
            //}

            return rep;
        }

        #endregion

        #region 提交
        /// <summary>
        /// 购物车提交
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-submit")]
        public CartSubmitResponse CartSubmit([FromBody]CartSubmitRequest req)
        {
            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartSubmitResponse();

            var param = new SubmitParam();
            param.Skus = req.Skus;
            param.Items = req.Items;
            param.Packages = req.Packages;
            param.UserAddressId = req.UserAddressId;

            var temp = cart.Submit();
            if (temp)
            {
                var payId = temp.Data;
                var platformId = 2;
                /*
                var onlinePay = GetOnlinePay(platformId);
                if (onlinePay != null)
                {
                    var payment = DefaultStorage.PaymentGet(payId);
                    //rep.Data = onlinePay.Pay(payment);
                }
                */
            }
            else
            {
                rep.Error("提交订单失败");
                return rep;
            }

            return rep;
        }

        #endregion

        [HttpGet("test")]
        public string Test()
        {
            var onlinePay = GetOnlinePay(2);

            if (onlinePay != null)
            {
                var payment = DefaultStorage.PaymentGet("1607261810160001");
                return onlinePay.Pay(payment);
            }
            return "";
        }

    }
}