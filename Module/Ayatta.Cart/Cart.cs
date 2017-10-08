using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Cart
{
    #region 购物车
    /// <summary>
    /// 购物车
    /// </summary>
    public sealed class Cart
    {
        private readonly string guid;
        private readonly int mediaId;
        private readonly Platform platform;

        private readonly DefaultStorage defaultStorage;
        private readonly IDistributedCache defaultCache;
        private readonly IDistributedCache cartCache;
        private readonly ILogger logger;

        /// <summary>
        /// 添加到购物车前触发
        /// </summary>
        //public Func<CacheData,Status> Adding { get; set; }

        /// <summary>
        /// 购物车数据处理完后触发
        /// </summary>
        public Action<CartData> Processed { get; set; }

        /// <summary>
        /// 购物车
        /// </summary>
        /// <param name="guid">购物车GUID</param>
        /// <param name="platform">Pc/Wap/App</param>
        /// <param name="mediaId">媒体Id</param>
        /// <param name="defaultStorage">默认数据存贮</param>
        /// <param name="defaultCache">默认缓存</param>
        /// <param name="cartCache">购物车缓存</param>
        /// <param name="logger">日志记录器</param>       
        public Cart(string guid, Platform platform, int mediaId, DefaultStorage defaultStorage, IDistributedCache defaultCache, IDistributedCache cartCache, ILogger logger)
        {
            this.guid = guid;
            this.platform = platform;
            this.mediaId = mediaId;
            this.defaultStorage = defaultStorage;
            this.defaultCache = defaultCache;
            this.cartCache = cartCache;
            this.logger = logger;
        }

        #region 获取购物车数据
        /// <summary>
        /// 获取购物车数据
        /// </summary>
        /// <returns></returns>
        public Status<CartData> GetData(bool? oversea = null)
        {
            try
            {
                var cacheData = GetCacheData();


                return ProcessCartData(cacheData, false);
            }
            catch (Exception e)
            {
                var message = e.Message;

                return new Status<CartData>(500, message, null);
            }
        }

        #endregion

        #region 商品操作
        /// <summary>
        /// 商品操作
        /// Status.Code <![CDATA[ 100>=code<200 商品相关信息 200>=code<300 卖家相关信息 ]]>
        /// </summary>
        /// <param name="operate">操作方式</param>        
        /// <param name="itemId">ItemId</param>
        /// <param name="skuId">SkuId</param>
        /// <param name="quantity">数量</param>
        public Status<CartData> ProductOpt(Operate operate, int itemId, int skuId = 0, int quantity = 1)
        {
            var status = new Status<CartData>();
            try
            {
                var stock = 0;
                #region 参数校验
                var key = $"product.item.mini.{itemId}";
                var item = defaultCache.Put(key, () => defaultStorage.ItemMiniGet(itemId), DateTime.Now.AddHours(2));

                if (item == null)
                {
                    status.Code = 100;
                    status.Message = $"商品({itemId})不存在";
                    return status;
                }
                if (item.Status != Prod.Status.Online)
                {
                    status.Code = 101;
                    status.Message = $"商品({itemId})已下架";
                    return status;
                }
                if (item.Stock < 1)
                {
                    status.Code = 102;
                    status.Message = $"商品({itemId})已售完";
                    return status;
                }
                stock = item.Stock;
                if (item.Skus != null && item.Skus.Count > 0)
                {
                    if (skuId < 1)
                    {
                        status.Code = 150;
                        status.Message = $"参数错误({itemId})(SkuId)";
                        return status;
                    }
                }
                var isSku = (itemId > 0 && skuId > 0);

                if (isSku)
                {
                    if (item.Skus == null || !item.Skus.Any())
                    {
                        status.Code = 150;
                        status.Message = $"商品Sku({skuId})不存在";
                        return status;
                    }
                    var sku = item.Skus.FirstOrDefault(x => x.Id == skuId);
                    if (sku == null)
                    {
                        status.Code = 150;
                        status.Message = $"商品Sku({skuId})不存在()";
                        return status;
                    }
                    if (sku.Status != Prod.Status.Online)
                    {
                        status.Code = 151;
                        status.Message = $"商品Sku({skuId})已下架";
                        return status;
                    }
                    if (sku.Stock < 1)
                    {
                        status.Code = 152;
                        status.Message = $"商品Sku({skuId})已售完";
                        return status;
                    }
                    stock = sku.Stock;
                }
                #endregion

                #region 处理购物车缓存数据
                var cacheData = GetCacheData();
                var basket = cacheData.GetBasket(item.SellerId);
                if (basket == null)
                {
                    var seller = defaultStorage.UserGet(item.SellerId);
                    if (seller != null)
                    {
                        cacheData.AddBasket(seller.Id, "", seller.Name);
                    }
                }
                basket = cacheData.GetBasket(item.SellerId);//确保已存在
                if (basket == null)
                {
                    status.Code = 200;
                    status.Message = $"卖家({item.SellerId})不存在";
                    return status;
                }

                switch (operate)
                {
                    case Operate.Increase:
                        {
                            if (isSku)
                            {
                                var q = quantity;
                                if (basket.Skus.ContainsKey(skuId))
                                {
                                    q += basket.Skus[skuId].Quantity;
                                }
                                if (q > stock)
                                {
                                    status.Code = 153;
                                    var dif = stock - basket.Skus[skuId].Quantity;
                                    status.Message = $"库存有限，最多还可购买{dif}件";
                                    if (dif == 0)
                                    {
                                        status.Code = 152;
                                        status.Message = $"已售完";
                                    }
                                    return status;
                                }
                                var result = ValidateLimitBuy(item.SellerId, cacheData.UserId, itemId, q);
                                if (result)//没有限购 或者没超出限购数量
                                {
                                    basket.AddSku(skuId, itemId, quantity);
                                }
                                else
                                {
                                    basket.SetSkuQuantity(skuId, itemId, result.Data);

                                    status.Code = 301;
                                    status.Message = $"限购{result.Data}件，已购买过{result.Extra}件 最多还可购买{result.Data - result.Extra}件";
                                }
                            }
                            else
                            {
                                var q = quantity;
                                if (basket.Items.ContainsKey(itemId))
                                {
                                    q += basket.Items[itemId].Quantity;
                                }
                                if (q > stock)
                                {
                                    status.Code = 153;
                                    var dif = stock - basket.Items[itemId].Quantity;
                                    status.Message = $"库存有限，最多还可购买{dif}件";
                                    if (dif == 0)
                                    {
                                        status.Code = 152;
                                        status.Message = $"已售完";
                                    }
                                    return status;
                                }
                                var result = ValidateLimitBuy(item.SellerId, cacheData.UserId, itemId, q);
                                if (result)//没有限购 或者没超出限购数量
                                {
                                    basket.AddItem(itemId, quantity);
                                }
                                else
                                {
                                    basket.SetItemQuantity(itemId, result.Data);

                                    status.Code = 301;
                                    status.Message = $"限购{result.Data}件，已购买过{result.Extra}件 最多还可购买{result.Data - result.Extra}件";
                                }
                            }
                        }
                        break;
                    case Operate.Decrease:
                        {
                            if (isSku)
                            {
                                basket.SubtractSku(skuId, quantity);
                            }
                            else
                            {
                                basket.SubtractItem(itemId, quantity);
                            }
                        }
                        break;
                    case Operate.Remove:
                        {
                            if (isSku)
                            {
                                basket.RemoveSku(skuId);
                            }
                            else
                            {
                                basket.RemoveItem(itemId);
                            }
                            if (basket.Skus.Count == 0 && basket.Items.Count == 0 && basket.Packages.Count == 0)
                            {
                                cacheData.Baskets.Remove(item.SellerId);
                            }
                        }
                        break;
                }
                #endregion               

                return ProcessCartData(cacheData, true);
            }
            catch (Exception e)
            {
                var message = e.Message;

                return new Status<CartData>(500, message, null);
            }
        }

        #endregion      

        #region 选择
        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="select">选择类型</param>
        /// <param name="param">选择参数值</param>
        /// <param name="selected">是否选中</param>
        /// <returns></returns>
        public Status<CartData> Select(Select select, int param, bool selected = true)
        {
            var status = new Status<CartData>(500);

            try
            {
                #region 处理购物车缓存数据
                var cacheData = GetCacheData();

                foreach (var basket in cacheData.Baskets)
                {
                    var bk = basket.Key;
                    var bv = basket.Value;

                    foreach (var sku in bv.Skus)
                    {
                        if (select == Ayatta.Cart.Select.All || (select == Ayatta.Cart.Select.Basket && bk == param))
                        {
                            sku.Value.Selected = selected;
                        }
                        else if (select == Ayatta.Cart.Select.Sku && sku.Key == param)
                        {
                            sku.Value.Selected = selected;
                        }
                    }

                    foreach (var item in bv.Items)
                    {
                        if (select == Ayatta.Cart.Select.All || (select == Ayatta.Cart.Select.Basket && bk == param))
                        {
                            item.Value.Selected = selected;
                        }
                        else if (select == Ayatta.Cart.Select.Item && item.Key == param)
                        {
                            item.Value.Selected = selected;
                        }
                    }

                }
                #endregion

                return ProcessCartData(cacheData, true);
            }
            catch (Exception e)
            {
                var message = e.Message;

                return new Status<CartData>(500, message, null);
            }
        }
        #endregion

        #region 清除

        /// <summary>
        /// 清除指定商品
        /// </summary>
        /// <param name="skus">待清除sku</param>
        /// <param name="items">待清除item</param>
        /// <param name="packages">待清除package</param>
        /// <returns></returns>
        public Status<CartData> Clean(int[] skus, int[] items, int[] packages)
        {
            var status = new Status<CartData>(500);
            if (skus == null && items == null && packages == null)
            {
                status.Message = "参数错误";
                return status;
            }
            try
            {
                #region 处理购物车缓存数据
                var cacheData = GetCacheData();

                foreach (var basket in cacheData.Baskets)
                {
                    foreach (var id in skus)
                    {
                        basket.Value.RemoveSku(id);
                    }
                    foreach (var id in items)
                    {
                        basket.Value.RemoveItem(id);
                    }
                    foreach (var id in packages)
                    {
                        basket.Value.RemovePackage(id);
                    }
                }
                #endregion

                return ProcessCartData(cacheData, true);
            }
            catch (Exception e)
            {
                var message = e.Message;

                return new Status<CartData>(500, message, null);
            }
        }
        #endregion

        #region 设置收货地址
        /// <summary>
        /// 设置收货地址
        /// </summary>
        /// <param name="userAddress">收货地址</param>
        /// <returns></returns>
        public Status<CartData> SetAddress(UserAddress userAddress)
        {
            if (userAddress == null)
            {
                return new Status<CartData>(500, "参数错误(userAddress)", null); ;
            }
            try
            {
                var cacheData = GetCacheData();

                cacheData.UserAddress = userAddress;

                return ProcessCartData(cacheData, true);
            }
            catch (Exception e)
            {
                var message = e.Message;

                return new Status<CartData>(500, message, null);
            }
        }

        #endregion

        #region 设置发票信息
        /// <summary>
        /// 设置发票信息
        /// </summary>
        /// <param name="invoice">发票信息</param>
        /// <returns></returns>
        public Status<CartData> SetInvoice(Invoice invoice)
        {
            if (invoice == null)
            {
                return new Status<CartData>(500, "参数错误(invoice)", null); ;
            }
            try
            {
                var cacheData = GetCacheData();

                cacheData.Invoice = invoice;

                return ProcessCartData(cacheData, true);
            }
            catch (Exception e)
            {
                var message = e.Message;

                return new Status<CartData>(500, message, null);
            }
        }

        #endregion

        #region 确认
        /*
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="skus">sku</param>
        /// <param name="items">item</param>
        /// <param name="packages">package</param>
        /// <param name="addressId">用户地址id</param>
        /// <returns></returns>
        public Status<CartData> Confirm(int[] skus, int[] items, int[] packages, int addressId = 0)
        {
            var status = new Status<CartData>(500);
            if (skus == null && items == null && packages == null)
            {
                status.Message = "参数错误";
                return status;
            }

            try
            {
                #region 处理购物车缓存数据
                var cacheData = GetCacheData();

                if (cacheData == null)
                {
                    status.Message = "获取或创建购物车缓存数据失败";
                    return status;
                }

                foreach (var basket in cacheData.Baskets)
                {
                    var array = basket.Value.Skus.Keys.Except(skus);

                    foreach (var id in array)
                    {
                        basket.Value.RemoveSku(id);
                    }

                    array = basket.Value.Items.Keys.Except(items);
                    foreach (var id in array)
                    {
                        basket.Value.RemoveItem(id);
                    }

                    array = basket.Value.Packages.Keys.Except(packages);
                    foreach (var id in array)
                    {
                        basket.Value.RemovePackage(id);
                    }
                }

                cacheData.AddressId = addressId;

                #endregion
                //不需要持久化到购物车缓存服务器
                return ProcessCartData(cacheData, false);
            }
            catch (Exception e)
            {
                var message = e.Message;

                return new Status<CartData>(500, message, null);
            }
        }
        */

        #endregion

        #region 提交        

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public Status<string> Submit()
        {
            var status = new Status<string>(500);

            try
            {
                var cacheData = GetCacheData();

                var cart = ProcessCartData(cacheData, false);
                if (!cart)
                {
                    status.Message = "处理购物车数据失败";
                    return status;
                }

                var data = cart.Data;

                var now = DateTime.Now;
                var amount = 0M;//合并支付总金额
                var orderIds = new List<string>();//成功生成订单的id集合

                var expiredOn = now.AddDays(3);

                //下单成功后 需要从购物车中删除的id集合
                var delSkus = new List<int>();
                var delItems = new List<int>();
                var delPackages = new List<int>();

                foreach (var basket in data.Baskets.Where(x => x.HasSelected))
                {
                    var order = new Order();
                    var id = Order.NewId(data.UserId, basket.SellerId);

                    order.Id = id;
                    order.Type = OrderType.Normal;
                    order.Quantity = basket.Quantity;
                    order.SubTotal = basket.SubTotal;
                    order.Freight = basket.Freight;
                    order.Discount = basket.Discount;
                    order.Total = basket.Total;
                    order.Paid = 0;
                    order.PayId = string.Empty;
                    order.PaidOn = null;
                    order.PointUse = 0;
                    order.PointRealUse = 0;
                    order.PointReward = 0;
                    order.Coupon = string.Empty;
                    order.CouponUse = 0;
                    order.GiftCard = string.Empty;
                    order.GiftCardUse = 0;
                    order.PromotionData = string.Empty;
                    order.Weight = 0;
                    order.ETicket = string.Empty;
                    order.IsVirtual = false;
                    order.IsBonded = false;
                    order.IsOversea = false;
                    order.PaymentType = PaymentType.Alipay;
                    order.PaymentData = "";
                    order.ShipmentType = ShipmentType.Express;
                    order.ShipmentData = "";
                    order.ExpiredOn = expiredOn;
                    order.ConsignedOn = null;
                    order.FinishedOn = null;

                    order.InvoiceType = 0;
                    order.InvoiceTitle = string.Empty;
                    order.InvoiceContent = string.Empty;
                    order.InvoiceStatus = 0;
                    if (data.Invoice != null)
                    {
                        order.InvoiceType = data.Invoice.Type;
                        order.InvoiceTitle = data.Invoice.Title;
                        order.InvoiceContent = data.Invoice.Content;
                    }

                    order.LogisticsNo = "";
                    order.LogisticsType = 0;
                    order.LogisticsCode = "";
                    order.LogisticsName = "";
                    order.Receiver = data.UserAddress.Consignee;
                    order.ReceiverPhone = data.UserAddress.Phone;
                    order.ReceiverMobile = data.UserAddress.Mobile;
                    order.ReceiverRegionId = data.UserAddress.RegionId;
                    order.ReceiverProvince = data.UserAddress.Province;
                    order.ReceiverCity = data.UserAddress.City;
                    order.ReceiverDistrict = data.UserAddress.District;
                    order.ReceiverStreet = data.UserAddress.Street;
                    order.ReceiverPostalCode = data.UserAddress.PostalCode;
                    order.BuyerId = data.UserId;
                    order.BuyerName = data.UserName;
                    order.BuyerFlag = 0;
                    order.BuyerMemo = "";
                    order.BuyerRated = false;
                    order.BuyerMessage = "";
                    order.SellerId = basket.SellerId;
                    order.SellerName = basket.SellerName;
                    order.SellerFlag = 0;
                    order.SellerMemo = "";
                    order.HasReturn = false;
                    order.HasRefund = false;
                    order.CancelId = 0;
                    order.CancelReason = "";
                    order.RelatedId = "";
                    order.MediaId = 0;
                    order.TraceCode = "";
                    order.IpAddress = "";
                    order.Extra = "";
                    order.Status = OrderStatus.WaitBuyerPay;
                    order.CreatedBy = "";
                    order.CreatedOn = now;
                    order.ModifiedBy = "";
                    order.ModifiedOn = now;


                    var i = 0;
                    var items = new List<OrderItem>();

                    foreach (var o in basket.Skus.Where(x => x.Selected))
                    {
                        var item = new OrderItem();

                        item.Id = order.Id + i.ToString("00");
                        item.OrderId = order.Id;
                        item.SpuId = o.SpuId;
                        item.ItemId = o.ItemId;
                        item.SkuId = o.Id;
                        item.CatgRId = o.CatgRId;
                        item.CatgMId = "";
                        item.CatgId = o.CatgId;
                        item.PackageId = 0;
                        item.PackageName = "";
                        item.Code = o.Code;
                        item.Name = o.Name;
                        item.Price = o.Price;
                        item.PriceShow = o.Price;
                        item.Quantity = o.Quantity;
                        item.Tax = 0;
                        item.Adjust = 0;
                        item.Discount = o.Discount;
                        item.Total = (o.Price * o.Quantity) - o.Discount;
                        item.TaxRate = 0;
                        item.Picture = o.Picture;
                        item.PropText = string.Join(";", o.PropTexts);
                        item.IsGift = false;
                        item.IsVirtual = o.IsVirtual;
                        item.IsService = false;
                        item.PromotionData = "";
                        item.ExpiredOn = expiredOn;
                        item.ConsignedOn = null;
                        item.FinishedOn = null;
                        item.LogisticsNo = "";
                        item.LogisticsName = "";
                        item.ReturnId = "";
                        item.ReturnStatus = 0;
                        item.RefundId = "";
                        item.RefundStatus = 0;
                        item.BuyerId = data.UserId;
                        item.BuyerName = data.UserName;
                        item.BuyerRated = false;
                        item.SellerId = basket.SellerId;
                        item.SellerName = basket.SellerName;
                        item.Extra = "";
                        item.Status = OrderStatus.WaitBuyerPay;
                        item.CreatedOn = now;
                        item.ModifiedBy = string.Empty;
                        item.ModifiedOn = now;

                        items.Add(item);

                        i++;
                    }
                    foreach (var o in basket.Items.Where(x => x.Selected))
                    {
                        var item = new OrderItem();

                        item.Id = order.Id + i.ToString("00");
                        item.OrderId = order.Id;
                        item.SpuId = o.SpuId;
                        item.ItemId = o.Id;
                        item.SkuId = 0;
                        item.CatgRId = o.CatgRId;
                        item.CatgMId = "";
                        item.CatgId = o.CatgId;
                        item.PackageId = 0;
                        item.PackageName = "";
                        item.Code = o.Code;
                        item.Name = o.Name;
                        item.Price = o.Price;
                        item.PriceShow = o.Price;
                        item.Quantity = o.Quantity;
                        item.Tax = 0;
                        item.Adjust = 0;
                        item.Discount = o.Discount;
                        item.Total = (o.Price * o.Quantity) - o.Discount;
                        item.TaxRate = 0;
                        item.Picture = o.Picture;
                        item.PropText = "";
                        item.IsGift = false;
                        item.IsVirtual = o.IsVirtual;
                        item.IsService = false;
                        item.PromotionData = "";
                        item.ExpiredOn = expiredOn;
                        item.ConsignedOn = null;
                        item.FinishedOn = null;
                        item.LogisticsNo = "";
                        item.LogisticsName = "";
                        item.ReturnId = "";
                        item.ReturnStatus = 0;
                        item.RefundId = "";
                        item.RefundStatus = 0;
                        item.BuyerId = data.UserId;
                        item.BuyerName = data.UserName;
                        item.BuyerRated = false;
                        item.SellerId = basket.SellerId;
                        item.SellerName = basket.SellerName;
                        item.Extra = "";
                        item.Status = OrderStatus.WaitBuyerPay;
                        item.CreatedOn = now;
                        item.ModifiedBy = string.Empty;
                        item.ModifiedOn = now;

                        items.Add(item);

                        i++;
                    }
                    order.Items = items;

                    var val = defaultStorage.OrderCreate(order);//生成订单
                    if (val)
                    {
                        orderIds.Add(id);

                        amount += basket.Total;

                        delSkus.AddRange(basket.Skus.Where(x => x.Selected).Select(x => x.Id));
                        delItems.AddRange(basket.Items.Where(x => x.Selected).Select(x => x.Id));
                        delPackages.AddRange(basket.Packages.Where(x => x.Selected).Select(x => x.Id));
                    }
                }

                if (orderIds.Count > 0)
                {
                    var payment = new Payment();

                    payment.Id = Payment.NewId();

                    payment.No = string.Empty;
                    payment.Type = 0;
                    payment.UserId = data.UserId;
                    payment.Method = 0;
                    payment.Amount = amount;
                    payment.Subject = "test";
                    payment.Message = "";
                    payment.RawData = "";
                    payment.BankId = 0;
                    payment.BankCode = string.Empty;
                    payment.BankName = string.Empty;
                    payment.BankCard = 0;
                    payment.PlatformId = 0;
                    payment.CardNo = string.Empty;
                    payment.CardPwd = string.Empty;
                    payment.CardAmount = 0;
                    payment.RelatedId = string.Join(",", orderIds);
                    payment.IpAddress = "106.2.161.2";
                    payment.Extra = "";
                    payment.Status = false;
                    payment.CreatedBy = "cart";
                    payment.CreatedOn = now;
                    payment.ModifiedBy = "cart";
                    payment.ModifiedOn = now;

                    var b = defaultStorage.PaymentCreate(payment);
                    if (b)
                    {
                        foreach (var basket in cacheData.Baskets)
                        {
                            foreach (var id in delSkus)
                            {
                                basket.Value.RemoveSku(id);
                            }
                            foreach (var id in delItems)
                            {
                                basket.Value.RemoveItem(id);
                            }
                            foreach (var id in delPackages)
                            {
                                basket.Value.RemovePackage(id);
                            }
                        }
                        SaveCacheData(cacheData);

                        status.Code = 0;
                        status.Data = payment.Id;
                        return status;
                    }
                    status.Message = "创建付款单失败";
                    return status;
                }

                status.Message = "创建订单失败";
                return status;
            }
            catch (Exception e)
            {
                status.Message = e.Message;
                return status;
            }
        }
        #endregion

        #region 清空购物车
        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <returns></returns>
        public Status Empty()
        {
            var status = new Status(500);
            try
            {
                cartCache.Remove(guid);
                status.Code = 0;
            }
            catch (Exception e)
            {
                status.Code = 500;
                status.Message = e.Message;
            }
            return status;
        }

        #endregion

        /// <summary>
        /// 合并临时购物车到用户购物车
        /// </summary>
        /// <param name="guid">临时购物车guid</param>
        public Status Merge(string guid)
        {
            try
            {
                var cacheData = GetCacheData();

                cacheData.Merge(cartCache.Get<CacheData>(guid));

                SaveCacheData(cacheData);

                cartCache.RemoveAsync(guid);

                return new Status(0);
            }
            catch (Exception e)
            {
                var message = e.Message;

                return new Status(500, message);
            }
        }

        #region 购物车数据处理
        /// <summary>
        /// 处理购物车数据
        /// </summary>
        /// <param name="data">购物车缓存数据</param>     
        /// <param name="saveCacheData">处理完购物车数据后 是否将购物车缓存数据保存到购物车缓存服务器</param>     
        /// <returns></returns>
        private Status<CartData> ProcessCartData(CacheData cacheData, bool saveCacheData)
        {
            try
            {

                var data = new CartData(cacheData, platform);

                ProcessStatus(data);//处理商品状态 库存及限购                

                ProcessPromotion(data);//处理促销信息

                OnProcessed(data);

                if (saveCacheData)
                {
                    SaveCacheData(data.ToCacheData());//将购物车数据转为购物车缓存数据并保存到购物车缓存服务器
                }
                return new Status<CartData>(0, data);
            }
            catch (Exception e)
            {
                var message = e.Message;

                return new Status<CartData>(500, message, null);
            }
        }

        /// <summary>
        /// 处理商品状态/库存及限购
        /// </summary>
        /// <param name="data">购物车数据</param>
        private void ProcessStatus(CartData data)
        {
            #region 处理商品状态 库存及限购
            var now = DateTime.Now;
            var date = now.ToString("yyyy-MM-dd");
            var expire = DateTime.Parse(date).AddDays(1);

            foreach (var basket in data.Baskets)
            {
                var key = $"promotion.limitbuy.{date}.{basket.SellerId}";

                var limits = defaultCache.Put(key, () => defaultStorage.PromotionLimitBuyList(basket.SellerId), expire);

                #region Skus
                foreach (var sku in basket.Skus)
                {
                    var item = defaultStorage.ItemMiniGet(sku.ItemId);
                    var x = item.Skus.FirstOrDefault(k => k.Id == sku.Id);
                    if (x != null)
                    {
                        sku.ItemId = item.Id;
                        sku.SpuId = item.SpuId;
                        sku.CatgId = item.CatgId;
                        sku.CatgRId = item.CatgRId;
                        sku.BrandId = item.BrandId;
                        sku.Code = x.Code;
                        sku.Name = item.Name;
                        sku.Price = x.Price;
                        sku.RetailPrice = x.RetailPrice;
                        sku.Picture = item.Picture;
                        sku.PropTexts = x.PropTexts;
                        sku.IsBonded = item.IsBonded;
                        sku.IsOversea = item.IsOversea;
                        sku.IsVirtual = item.IsVirtual;
                        sku.Max = x.Stock;

                        if (x.Status == Prod.Status.Online)
                        {
                            if (x.Stock <= 0)
                            {
                                sku.Selected = false;
                                sku.Message = "已售完";
                                sku.Status = CartData.Basket.Status.SoldOut;
                                continue;
                            }
                            if (x.Stock <= sku.Quantity)
                            {
                                sku.Quantity = x.Stock;
                            }
                            sku.Status = CartData.Basket.Status.Online;
                            var limit = limits?.FirstOrDefault(d => d.ItemId == item.Id);

                            if (limit != null && limit.Value > 0)
                            {
                                sku.Limit = limit.Value;
                                if (sku.Quantity >= sku.Limit)
                                {
                                    sku.Max = sku.Limit;
                                    sku.Quantity = sku.Limit;
                                    sku.Message = $"每人限购{sku.Limit}件";
                                }
                            }
                        }
                    }
                    else
                    {
                        sku.Selected = false;
                        sku.Message = "已下架";
                        sku.Status = CartData.Basket.Status.Offline;
                    }
                }
                #endregion

                #region Items

                foreach (var item in basket.Items)
                {
                    var x = defaultStorage.ItemMiniGet(item.Id);
                    if (x != null)
                    {
                        item.SpuId = x.SpuId;
                        item.CatgId = x.CatgId;
                        item.CatgRId = x.CatgRId;
                        item.BrandId = x.BrandId;
                        item.Code = x.Code;
                        item.Name = x.Name;
                        item.Price = x.Price;
                        item.RetailPrice = x.RetailPrice;

                        item.Picture = x.Picture;
                        item.IsBonded = x.IsBonded;
                        item.IsOversea = x.IsOversea;
                        item.IsVirtual = x.IsVirtual;
                        item.Max = x.Stock;

                        if (x.Status == Prod.Status.Online)
                        {
                            if (x.Stock <= 0)
                            {
                                item.Selected = false;
                                item.Message = "已售完";
                                item.Status = CartData.Basket.Status.SoldOut;
                                continue;
                            }
                            if (x.Stock <= item.Quantity)
                            {
                                item.Quantity = x.Stock;
                            }

                            item.Status = CartData.Basket.Status.Online;
                            var limit = limits?.FirstOrDefault(d => d.ItemId == item.Id);

                            if (limit != null && limit.Value > 0)
                            {
                                item.Limit = limit.Value;
                                if (item.Quantity >= item.Limit)
                                {
                                    item.Max = item.Limit;
                                    item.Quantity = item.Limit;
                                    item.Message = $"每人限购{item.Limit}件";
                                }
                            }
                        }
                    }
                    else
                    {
                        item.Selected = false;
                        item.Message = "已下架";
                        item.Status = CartData.Basket.Status.Offline;
                    }
                }
                #endregion

                #region Packages
                foreach (var package in basket.Packages)
                {

                }
                #endregion

            }
            #endregion
        }

        /// <summary>
        /// 处理购物车促销数据
        /// </summary>
        /// <param name="data">购物车数据</param>
        private void ProcessPromotion(CartData data)
        {
            var now = DateTime.Now;
            var date = now.ToString("yyyy-MM-dd");
            var expire = DateTime.Parse(date).AddDays(1);
            foreach (var basket in data.Baskets)
            {
                #region 处理特价
                var key = $"promotion.specialprice.{date}.{basket.SellerId}";
                var specialPrices = defaultCache.Put(key, () => defaultStorage.PromotionSpecialPriceList(basket.SellerId), expire);
                if (specialPrices != null && specialPrices.Any())
                {
                    foreach (var specialPrice in specialPrices)
                    {
                        if (specialPrice.IsValid(platform))
                        {
                            foreach (var sku in basket.Skus)
                            {
                                var magic = specialPrice.Contains(sku.ItemId, sku.Id);
                                if (magic.First)
                                {
                                    var v = magic.Second;
                                    var discount = new Discount(Promotion.Type.A, specialPrice.Id, specialPrice.Name);
                                    switch (specialPrice.Type)
                                    {
                                        case Promotion.SpecialPriceType.A:
                                            var amount = sku.Price - (sku.Price * v);
                                            var description = $"限时特价促销 原价{sku.Price},打{v}折,优惠{amount}";

                                            discount.Pass = true;
                                            discount.Amount = amount;
                                            discount.Description = description;

                                            sku.Price = sku.Price * v;
                                            sku.Discount = amount;

                                            break;
                                        case Promotion.SpecialPriceType.B:
                                            amount = sku.Price - v;
                                            description = $"限时特价促销 原价{sku.Price},减{v},优惠{amount}";

                                            discount.Pass = true;
                                            discount.Amount = amount;
                                            discount.Description = description;

                                            sku.Price = sku.Price - v;
                                            sku.Discount = amount;

                                            break;
                                        case Promotion.SpecialPriceType.C:
                                            if (sku.Price > v)
                                            {
                                                amount = sku.Price - v;
                                                description = $"限时特价促销 原价{sku.Price},促销价{v},优惠{amount}";

                                                discount.Pass = true;
                                                discount.Amount = amount;
                                                discount.Description = description;

                                                sku.Price = v;
                                                sku.Discount = amount;
                                            }
                                            break;
                                    }
                                    if (sku.Selected)
                                    {
                                        basket.Discounts.Add(discount);
                                    }

                                }
                            }

                            foreach (var item in basket.Items)
                            {
                                var magic = specialPrice.Contains(item.Id);
                                if (magic.First)
                                {
                                    var v = magic.Second;
                                    var discount = new Discount(Promotion.Type.A, specialPrice.Id, specialPrice.Name);
                                    switch (specialPrice.Type)
                                    {
                                        case Promotion.SpecialPriceType.A:
                                            var amount = item.Price - (item.Price * v);
                                            var description = $"限时特价促销 原价{item.Price},打{v}折,优惠{amount}";

                                            discount.Pass = true;
                                            discount.Amount = amount;
                                            discount.Description = description;

                                            item.Price = item.Price * v;
                                            item.Discount = amount;

                                            break;
                                        case Promotion.SpecialPriceType.B:
                                            amount = item.Price - v;
                                            description = $"限时特价促销 原价{item.Price},减{v},优惠{amount}";

                                            discount.Pass = true;
                                            discount.Amount = amount;
                                            discount.Description = description;

                                            item.Price = item.Price - v;
                                            item.Discount = amount;

                                            break;
                                        case Promotion.SpecialPriceType.C:
                                            if (item.Price > v)
                                            {
                                                amount = item.Price - v;
                                                description = $"限时特价促销 原价{item.Price},促销价{v},优惠{amount}";

                                                discount.Pass = true;
                                                discount.Amount = amount;
                                                discount.Description = description;

                                                item.Price = v;
                                                item.Discount = amount;
                                            }
                                            break;
                                    }
                                    if (item.Selected)
                                    {
                                        basket.Discounts.Add(discount);
                                    }
                                }
                            }

                            foreach (var package in basket.Packages.Where(x => x.Selected))
                            {

                            }
                        }
                    }
                }
                #endregion

                #region 处理店铺优惠
                key = $"promotion.normal.{date}.{basket.SellerId}";  //店铺优惠key               
                var promotions = cartCache.Put(key, () => defaultStorage.PromotionActivityList(basket.SellerId), expire);
                if (promotions != null && promotions.Any())
                {
                    var tmp = promotions.OrderBy(x => x.Global);//同时存在单品活动和店铺活动时 优先处理单品活动
                    var ids = tmp.SelectMany(x => x.Items).ToList();//所有单品活动中的商品
                    foreach (var promotion in promotions)
                    {
                        if (promotion.IsValid(platform))
                        {
                            var amount = 0m;
                            var quantity = 0;

                            foreach (var sku in basket.Skus.Where(x => x.Selected))
                            {
                                if (promotion.Global && !ids.Contains(sku.ItemId)) //店铺活动 需要排除已参与了商品活动的商品
                                {
                                    quantity += sku.Quantity;
                                    amount += sku.Price * sku.Quantity;
                                }
                                else if (!promotion.Global && promotion.Items.Contains(sku.ItemId))
                                {
                                    quantity += sku.Quantity;
                                    amount += sku.Price * sku.Quantity;
                                }
                            }

                            foreach (var item in basket.Items.Where(x => x.Selected))
                            {
                                if (promotion.Global && !ids.Contains(item.Id)) //店铺活动 需要排除已参与了商品活动的商品
                                {
                                    quantity += item.Quantity;
                                    amount += item.Price * item.Quantity;
                                }
                                else if (!promotion.Global && promotion.Items.Contains(item.Id))
                                {
                                    quantity += item.Quantity;
                                    amount += item.Price * item.Quantity;
                                }
                            }

                            foreach (var package in basket.Packages.Where(x => x.Selected))
                            {

                                if (promotion.Global && !ids.Contains(package.MainId)) //店铺活动 需要排除已参与了商品活动的商品
                                {
                                    quantity += package.Quantity;
                                    amount += package.Price * package.Quantity;
                                }
                                else if (!promotion.Global && promotion.Items.Contains(package.MainId))
                                {
                                    quantity += package.Quantity;
                                    amount += package.Price * package.Quantity;
                                }

                                foreach (var o in package.Subs)
                                {
                                    if (promotion.Global && !ids.Contains(o.ItemId)) //店铺活动 需要排除已参与了商品活动的商品
                                    {
                                        quantity += 1;
                                        amount += o.Price;
                                    }
                                    else if (!promotion.Global && promotion.Items.Contains(o.ItemId))
                                    {
                                        quantity += 1;
                                        amount += o.Price;
                                    }
                                }
                            }

                            var rule = promotion.MatchRule(amount, quantity);
                            if (rule != null)
                            {
                                if (data.UserAddress != null)
                                {
                                    //免运费排除在外的地区多个以,分隔
                                    var array = promotion.FreightFreeExclude.Split(',');
                                    if (promotion.FreightFree && !array.Contains(data.UserAddress.RegionId))
                                    {
                                        basket.FreightFree = true;
                                    }
                                }

                                var discount = new Discount(Promotion.Type.B, promotion.Id, promotion.Name);
                                if (promotion.Type == 1)
                                {
                                    discount.Amount = amount * rule.Discount;
                                    var description = $"店铺促销 满{rule.Threshold}件,打{rule.Discount}折,优惠{discount.Amount}";
                                    discount.Description = description;
                                }
                                else
                                {
                                    discount.Amount = rule.Discount;
                                    var description = $"店铺促销 满{rule.Threshold}元,减{rule.Discount}元,优惠{discount.Amount}";
                                    discount.Description = description;
                                }
                                basket.Discounts.Add(discount);

                                if (rule.Gifts != null)
                                {
                                    foreach (var gift in rule.Gifts)
                                    {
                                        var o = new Gift();
                                        o.SkuId = gift.SkuId;
                                        o.ItemId = gift.ItemId;
                                        o.Name = gift.Name;
                                        o.Quantity = gift.Quantity;
                                        o.PropText = gift.PropText;

                                        basket.Gifts.Add(o);
                                    }
                                }

                                if (rule.Coupons != null)
                                {
                                    foreach (var coupon in rule.Coupons)
                                    {
                                        var o = new Coupon();

                                        o.Global = true;
                                        o.Value = coupon.Value;
                                        o.Count = coupon.Count;
                                        o.Platform = coupon.Platform;
                                        o.StartedOn = coupon.StartedOn;
                                        o.StoppedOn = coupon.StoppedOn;
                                        o.Limit = coupon.Limit;
                                        o.LimitVal = coupon.LimitVal;

                                        //basket.Coupons.Add(o);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 处理运费
                if (!basket.FreightFree && data.UserAddress != null)
                {

                }
                #endregion

                #region 处理税费

                #endregion

                #region 处理购物车促销
                key = $"promotion.cart.{date}.{basket.SellerId}";  //购物车优惠key               
                var cartPromotions = cartCache.Put(key, () => defaultStorage.PromotionCartActivityList(basket.SellerId), expire);
                if (cartPromotions != null && cartPromotions.Any())
                {
                    foreach (var cartPrmotion in cartPromotions)
                    {
                        var isValid = cartPrmotion.IsValid(platform);
                        var isMatch = cartPrmotion.Match(data.UserGrade, data.UserId, mediaId, data.UserAddress.RegionId);
                        if (isValid && isMatch)
                        {
                            var done = true;//必须满足所有规则
                            foreach (var rule in cartPrmotion.Rules.OrderBy(x => x.Priority))
                            {
                                if (rule.CalcType == Promotion.CalcType.A)
                                {
                                    var val = rule.AsValueA();
                                    if (val.IsValid)
                                    {
                                        var opt = val.CountOpt;//订单比较符 < = >
                                        var orderCount = 0;//验证订单数
                                        if (opt == '>')
                                        {
                                            if (orderCount < val.CountParam)
                                            {
                                                done = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            //满足所有规则
                            if (done)
                            {
                                var discount = new Discount(Promotion.Type.D, cartPrmotion.Id, cartPrmotion.Name);
                                if (cartPrmotion.DiscountOn == Promotion.DiscountOn.OrderTotal)//促销折扣/减免金额作用于 订单总金额
                                {
                                    if (cartPrmotion.Type == 1)
                                    {
                                        discount.Amount = basket.Total * cartPrmotion.DiscountValue;
                                        var description = $"购物车促销 订单总金额 打{cartPrmotion.DiscountValue}折,优惠{discount.Amount}";
                                        discount.Description = description;
                                    }
                                    else
                                    {
                                        discount.Amount = basket.Total - cartPrmotion.DiscountValue;
                                        var description = $"购物车促销 订单总金额 减{cartPrmotion.DiscountValue}元,优惠{discount.Amount}";
                                        discount.Description = description;
                                    }
                                }
                                else if (cartPrmotion.DiscountOn == Promotion.DiscountOn.OrderSubTotal)//促销折扣/减免金额作用于 订单商品总金额
                                {
                                    if (cartPrmotion.Type == 1)
                                    {
                                        discount.Amount = basket.SubTotal * cartPrmotion.DiscountValue;
                                        var description = $"购物车促销 商品总金额 打{cartPrmotion.DiscountValue}折,优惠{discount.Amount}";
                                        discount.Description = description;
                                    }
                                    else
                                    {
                                        discount.Amount = basket.SubTotal - cartPrmotion.DiscountValue;
                                        var description = $"购物车促销 商品总金额 减{cartPrmotion.DiscountValue}元,优惠{discount.Amount}";
                                        discount.Description = description;
                                    }
                                }
                                else if (cartPrmotion.DiscountOn == Promotion.DiscountOn.Freight)//促销折扣/减免金额作用于 运费
                                {
                                    if (cartPrmotion.Type == 1)
                                    {
                                        discount.Amount = basket.Freight * cartPrmotion.DiscountValue;
                                        var description = $"购物车促销 运费 打{cartPrmotion.DiscountValue}折,优惠{discount.Amount}";
                                        discount.Description = description;
                                    }
                                    else
                                    {

                                        if (basket.Freight - cartPrmotion.DiscountValue > 0)
                                        {
                                            discount.Amount = basket.Freight - cartPrmotion.DiscountValue;
                                        }
                                        else
                                        {
                                            basket.Freight = 0;
                                            discount.Amount = cartPrmotion.DiscountValue - basket.Freight;
                                        }
                                        var description = $"购物车促销 运费 减{cartPrmotion.DiscountValue}元,优惠{discount.Amount}";
                                        discount.Description = description;
                                    }
                                }
                                else if (cartPrmotion.DiscountOn == Promotion.DiscountOn.Tax)//促销折扣/减免金额作用于 税费
                                {
                                    if (cartPrmotion.Type == 1)
                                    {
                                        discount.Amount = basket.Tax * cartPrmotion.DiscountValue;
                                        var description = $"购物车促销 税费 打{cartPrmotion.DiscountValue}折,优惠{discount.Amount}";
                                        discount.Description = description;
                                    }
                                    else
                                    {

                                        if (basket.Tax - cartPrmotion.DiscountValue > 0)
                                        {
                                            discount.Amount = basket.Tax - cartPrmotion.DiscountValue;
                                        }
                                        else
                                        {
                                            basket.Tax = 0;
                                            discount.Amount = cartPrmotion.DiscountValue - basket.Tax;
                                        }
                                        var description = $"购物车促销 税费 减{cartPrmotion.DiscountValue}元,优惠{discount.Amount}";
                                        discount.Description = description;
                                    }
                                }
                                else if (cartPrmotion.DiscountOn == Promotion.DiscountOn.Price)//促销折扣/减免金额作用于 商品价格
                                {
                                    foreach (var sku in basket.Skus.Where(x => x.Selected))
                                    {
                                        isMatch = cartPrmotion.Match(sku.Id, sku.ItemId, sku.CatgId, sku.BrandId);
                                        if (isMatch && cartPrmotion.Type == 1)
                                        {
                                            var v = cartPrmotion.DiscountValue;
                                            var amount = sku.Price - (sku.Price * v);
                                            var description = $"购物车促销 原价{sku.Price},打{v}折,优惠{amount}";

                                            discount.Pass = true;
                                            discount.Amount = amount;
                                            discount.Description = description;

                                            sku.Price = sku.Price * v;
                                            sku.Discount += amount;
                                        }
                                        if (isMatch && cartPrmotion.Type == 0)
                                        {
                                            var v = cartPrmotion.DiscountValue;
                                            var amount = sku.Price - v;
                                            var description = $"购物车促销 原价{sku.Price},减{v},优惠{amount}";

                                            discount.Pass = true;
                                            discount.Amount = amount;
                                            discount.Description = description;

                                            sku.Price = sku.Price - v;
                                            sku.Discount += amount;
                                        }
                                    }
                                }

                                basket.Discounts.Add(discount);
                            }
                        }
                    }
                }
                #endregion
            }
        }


        /// <summary>
        /// 限购检查
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <param name="buyerId">卖家Id</param>
        /// <param name="itemId">ItemId</param>    
        /// <param name="quantity">数量</param>
        /// <returns>返回是否没有超出限购 限购数量 可购数量</returns>
        private Result<int, int> ValidateLimitBuy(int sellerId, int buyerId, int itemId, int quantity)
        {
            var result = new Result<int, int>(true);
            var now = DateTime.Now;
            var date = now.ToString("yyyy-MM-dd");
            var expire = DateTime.Parse(date).AddDays(1);
            var key = $"promotion.limitbuy.{date}.{sellerId}";

            var limits = defaultCache.Put(key, () => defaultStorage.PromotionLimitBuyList(sellerId), expire);
            if (limits == null)
            {
                return result;
            }
            var limit = limits.FirstOrDefault(x => x.ItemId == itemId);

            if (limit != null)
            {
                result.Data = limit.Value;//限购数量
                //Todo 验证订单数
                var count = 0;//已购买数量
                result.Extra = count;//已购买数量

                if (count + quantity > limit.Value)
                {
                    result.Status = false;
                }
            }
            return result;
        }

        #endregion

        #region Private

        /// <summary>
        /// 从购物车缓存服务器获取购物车缓存数据
        /// 如果不存在则新建购物车缓存数据并保存到购物车缓存服务器
        /// </summary>
        /// <returns></returns>
        private CacheData GetCacheData()
        {
            var cacheData = cartCache.Put(guid, () => new CacheData(), DateTime.Now.AddDays(1));
            if (cacheData.UserId == 0)
            {
                var user = defaultStorage.UserGet(guid);
                if (user == null) return cacheData;
                cacheData.UserId = user.Id;
                cacheData.UserName = user.Name;
                cacheData.UserGrade = user.Grade;

                var addresses = defaultStorage.UserAddressList(user.Id, 0);
                cacheData.UserAddress = addresses.FirstOrDefault(x => x.IsDefault);
            }
            return cacheData;
        }

        /// <summary>
        /// 保存购物车缓存数据到购物车缓存服务器
        /// </summary>
        /// <param name="data">购物车缓存数据</param>
        private void SaveCacheData(CacheData data)
        {
            cartCache.Set(guid, data, DateTime.Now.AddDays(1));
        }

        /// <summary>
        /// 购物车数据处理完后触发
        /// </summary>
        /// <param name="data">购物车数据</param>
        private void OnProcessed(CartData data)
        {
            var handler = Processed;
            handler?.Invoke(data);
        }

        ///// <summary>
        ///// 获取用户选中的收货地址信息
        ///// </summary>
        ///// <param name="userId">用户Id</param>
        ///// <param name="userAddressId">用户地址Id</param>
        ///// <returns></returns>
        //private UserAddress GetUserAddress(int userId, int userAddressId)
        //{
        //    var addresses = defaultStorage.UserAddressList(userId);
        //    return addresses.FirstOrDefault(x => x.Id == userAddressId);
        //}
        #endregion

    }
    #endregion
}