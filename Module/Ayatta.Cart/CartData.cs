using System;
using System.Linq;
using Ayatta.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Cart
{
    #region 购物车数据
    /// <summary>
    /// 购物车数据
    /// </summary>
    public class CartData
    {
        #region


        /// <summary>
        /// 买家UserId
        /// </summary>
        [JsonIgnore]
        public int UserId { get; }

        /// <summary>
        /// 买家UserName
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public string UserName { get; }

        /// <summary>
        /// 买家级别
        /// </summary>
        [JsonIgnore]
        public UserGrade UserGrade { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        [JsonIgnore]
        public Platform Platform { get; }

        /// <summary>
        /// 是否
        /// </summary>
        public bool Oversea { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public UserAddress UserAddress { get; }

        /// <summary>
        /// 发票信息
        /// </summary>
        public Invoice Invoice { get; }

        /// <summary>
        /// 报关所需信息
        /// </summary>
        public Identity Identity { get; set; }

        /// <summary>
        /// 按卖家拆分的购物蓝
        /// </summary>
        public IList<Basket> Baskets { get; }

        /// <summary>
        /// 是否全选
        /// </summary>
        public bool Selected
        {
            get
            {
                return Baskets.All(x => x.Selected);
            }
        }

        /// <summary>
        /// 已选
        /// </summary>
        public Summary Summary
        {
            get
            {
                var i = 0;
                var total = 0m;
                foreach (var basket in Baskets)
                {
                    foreach (var sku in basket.Skus)
                    {
                        if (sku.Selected)
                        {
                            i++;
                        }
                    }

                    foreach (var item in basket.Items)
                    {
                        if (item.Selected)
                        {
                            i++;
                        }
                    }
                    foreach (var package in basket.Packages)
                    {
                        if (package.Selected)
                        {
                            i++;
                        }
                    }
                    total += basket.SubTotal;
                }
                var summary = new Summary();
                summary.Count = i;
                summary.Total = total;
                summary.Discount = Baskets.Sum(x => x.Discount);
                return summary;
            }
        }

        internal bool NeedAddress
        {
            get
            {
                return Baskets.Any(x => x.NeedAddress);
            }
        }

        #region private
        internal CartData(CacheData data, Platform platform)
        {
            Platform = platform;

            UserId = data.UserId;
            UserName = data.UserName;
            UserGrade = data.UserGrade;
            UserAddress = data.UserAddress;

            Invoice = data.Invoice;
            Identity = data.Identity;

            Baskets = GetBaskets(data.Baskets);
        }
        private static IList<Basket> GetBaskets(IDictionary<int, CacheData.Basket> baskets)
        {
            var list = new List<Basket>();
            foreach (var basket in baskets)
            {
                var o = new Basket();
                var key = basket.Key;
                var value = baskets[key];

                o.Name = value.Name;
                o.SellerId = key;
                o.SellerName = value.SellerName;

                o.Skus = value.Skus.Select(x => new Basket.Sku()
                {
                    Id = x.Key,
                    ItemId = x.Value.ItemId,
                    Quantity = x.Value.Quantity,
                    Selected = x.Value.Selected
                }).ToList();
                o.Items = value.Items.Select(x => new Basket.Item()
                {
                    Id = x.Key,
                    Quantity = x.Value.Quantity,
                    Selected = x.Value.Selected
                }).ToList();

                if (o.Skus.Count > 0 || o.Items.Count > 0 || o.Packages.Count > 0)
                {
                    list.Add(o);//保证数据有效
                }
            }
            return list;
        }


        /// <summary>
        /// 将购物车数据转成购物车缓存数据
        /// </summary>
        /// <returns></returns>
        internal CacheData ToCacheData()
        {
            var data = new CacheData();
            data.UserId = UserId;
            data.UserName = UserName;
            data.UserGrade = UserGrade;

            data.Timestamp = DateTime.Now;

            var baskets = new Dictionary<int, CacheData.Basket>();
            foreach (var basket in Baskets)
            {
                var key = basket.SellerId;
                var val = new CacheData.Basket();
                val.Name = basket.Name;
                val.SellerName = basket.SellerName;
                val.Skus = basket.Skus.ToDictionary(x => x.Id, x => new CacheData.Basket.Sku { ItemId = x.ItemId, Quantity = x.Quantity, Selected = x.Selected });
                val.Items = basket.Items.ToDictionary(x => x.Id, x => new CacheData.Basket.Item { Quantity = x.Quantity, Selected = x.Selected });
                val.Packages = basket.Packages.ToDictionary(x => x.Id, x => new CacheData.Basket.Package { Quantity = x.Quantity, Selected = x.Selected });

                if (val.Skus.Count > 0 || val.Items.Count > 0 || val.Packages.Count > 0)
                {
                    baskets.Add(key, val);//保证数据有效
                }
            }

            data.Baskets = baskets;
            data.UserAddress = UserAddress;
            data.Invoice = Invoice;
            data.Identity = Identity;
            return data;
        }

        #endregion

        #endregion       

        #region Basket

        /// <summary>
        /// 购物篮
        /// </summary>
        public class Basket
        {
            /// <summary>
            /// 卖家店铺名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 卖家Id
            /// </summary>
            public int SellerId { get; set; }

            /// <summary>
            /// 卖家名称
            /// </summary>
            public string SellerName { get; set; }

            /// <summary>
            /// 商品Sku
            /// </summary>
            public IList<Sku> Skus { get; set; }

            /// <summary>
            /// 商品
            /// </summary>
            public IList<Item> Items { get; set; }

            /// <summary>
            /// 套餐
            /// </summary>
            public IList<Package> Packages { get; set; }

            /// <summary>
            /// 是否选中
            /// </summary>
            public bool Selected
            {
                get
                {
                    var total = Skus.Count + Items.Count + Packages.Count;
                    var selected = Skus.Count(x => x.Selected) + Items.Count(x => x.Selected) + Packages.Count(x => x.Selected);
                    var disabled = Skus.Count(x => x.Status != 0) + Items.Count(x => x.Status != 0) + Packages.Count(x => x.Status != 0);

                    return total - disabled == selected;
                }
            }

            public bool HasSelected
            {
                get
                {
                    return Skus.Any(x => x.Selected) || Items.Any(x => x.Selected) || Packages.Any(x => x.Selected);
                }
            }

            /// <summary>
            /// 商品总件数
            /// </summary>
            public int Quantity
            {
                get
                {
                    var v = Skus.Where(x => x.Selected).Sum(sku => sku.Quantity) + Items.Where(x => x.Selected).Sum(item => item.Quantity);
                    foreach (var package in Packages.Where(x => x.Selected))
                    {
                        if (package.Fixed)
                        {
                            v += package.Quantity + package.Subs.Count;
                        }
                        else
                        {
                            v += package.Quantity + package.Subs.Count(x => x.Selected);
                        }
                    }
                    return v;
                }
            }

            /// <summary>
            /// 商品总金额
            /// </summary>
            public decimal SubTotal
            {
                get
                {
                    var v = Skus.Where(x => x.Selected).Sum(sku => sku.Price * sku.Quantity) + Items.Where(x => x.Selected).Sum(item => item.Price * item.Quantity);
                    foreach (var package in Packages.Where(x => x.Selected))
                    {
                        //v += package.Price * package.Quantity;
                    }
                    return v;
                }
            }

            /// <summary>
            /// 税费
            /// </summary>
            public decimal Tax { get; set; }

            /// <summary>
            /// 是否免税费（税费由商家承担）
            /// </summary>
            internal bool TaxFree { get; set; }

            /// <summary>
            /// 运费
            /// </summary>
            public decimal Freight { get; set; }

            /// <summary>
            /// 是否免运费
            /// </summary>
            internal bool FreightFree { get; set; }

            /// <summary>
            /// 优惠总金额
            /// </summary>
            public decimal Discount => Discounts.Where(x => !x.Pass).Sum(x => x.Amount);

            /// <summary>
            /// 订单总金额(商品总金额+运费+税费-优惠总金额)
            /// </summary>
            public decimal Total => SubTotal + Freight + Tax - Discount;


            /// <summary>
            /// 赠品
            /// </summary>
            public IList<Gift> Gifts { get; set; }

            ///// <summary>
            ///// 使用的优惠券
            ///// </summary>
            //public IList<Coupon> CouponUsed { get; private set; }

            /// <summary>
            /// 赠送的优惠券
            /// </summary>
            public IList<Promotion.Activity.Coupon> Coupons { get; set; }

            /// <summary>
            /// 优惠信息
            /// </summary>
            public List<Discount> Discounts { get; set; }


            /// <summary>
            /// 是否
            /// </summary>
            internal bool NeedAddress
            {
                get
                {
                    return Skus.Any(x => x.IsVirtual) || Items.Any(x => x.IsVirtual) || Packages.Any(x => x.IsVirtual);
                }
            }

            #region Constructor

            public Basket()
            {
                Skus = new List<Sku>();
                Items = new List<Item>();
                Packages = new List<Package>();

                //Gifts = new List<Promotion.Normal.Gift>();
                Coupons = new List<Promotion.Activity.Coupon>();
                Discounts = new List<Discount>();
            }

            #endregion

            #region Method

            public bool Validate()
            {
                return true;
            }

            #endregion

            #region 商品状态
            /// <summary>
            /// 商品状态
            /// </summary>
            public enum Status : byte
            {
                /// <summary>
                /// 正常售卖中
                /// </summary>
                Online = 0,

                /// <summary>
                /// 已售完
                /// </summary>
                SoldOut = 1,

                /// <summary>
                /// 已下线
                /// </summary>
                Offline = 2,

                /// <summary>
                /// 不支持收货地址
                /// </summary>
                X = 3
            }
            #endregion

            #region 商品/商品Sku/搭配组合套餐
            public abstract class Base
            {
                /// <summary>
                /// Id
                /// </summary>
                public int Id { get; set; }

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 销售价格
                /// </summary>
                public decimal Price { get; set; }

                /// <summary>
                /// 商品市场建议零售价格
                /// </summary>
                /// <returns></returns>
                public decimal RetailPrice { get; set; }

                /// <summary>
                /// 数量
                /// </summary>
                public int Quantity { get; set; }

                /// <summary>
                /// 图片
                /// </summary>
                public string Picture { get; set; }

                /// <summary>
                /// 优惠金额
                /// </summary>
                public decimal Discount { get; set; }

                ///<summary>
                /// 是否为保税仓发货
                ///</summary>
                public bool IsBonded { get; set; }

                ///<summary>
                /// 是否为海外直邮
                ///</summary>
                public bool IsOversea { get; set; }

                /// <summary>
                /// 是否为虚拟产品
                /// </summary>
                public bool IsVirtual { get; set; }

                /// <summary>
                /// 是否选中
                /// </summary>
                public bool Selected { get; set; }

                /// <summary>
                /// 最大可购买数量
                /// </summary>
                public int Max { get; set; }

                /// <summary>
                /// 状态 0为正常
                /// </summary>
                public Status Status { get; set; }

                /// <summary>
                /// 消息
                /// </summary>
                public string Message { get; set; }

            }

            /// <summary>
            /// 商品
            /// </summary>
            public class Item : Base
            {
                ///<summary>
                /// SpuId
                ///</summary>
                public int SpuId { get; set; }

                ///<summary>
                /// 最小类目id
                ///</summary>
                public int CatgId { get; set; }

                ///<summary>
                /// 根类目id
                ///</summary>
                public int CatgRId { get; set; }

                ///<summary>
                /// 中间类目id
                ///</summary>
                //public string CatgMId { get; set; }

                /// <summary>
                /// 品牌Id
                /// </summary>                
                public int BrandId { get; set; }

                ///<summary>
                /// 商品编号 商家设置的外部id
                ///</summary>
                public string Code { get; set; }

                /// <summary>
                /// 限购数量 0为不限 大于0为限购数量
                /// </summary>
                public int Limit { get; set; }

            }

            /// <summary>
            /// 商品Sku
            /// </summary>
            public class Sku : Item
            {
                /// <summary>
                /// 商品Id
                /// </summary>
                public int ItemId { get; set; }

                /// <summary>
                /// 销售属性
                /// </summary>
                public string[] PropTexts { get; set; }

            }

            /// <summary>
            /// 搭配组合套餐
            /// </summary>
            public class Package : Base
            {
                /// <summary>
                /// 主商品Id
                /// </summary>
                public int MainId { get; set; }

                /// <summary>
                /// 固定组合套餐 商品打包成套餐销售，消费者打包购买
                /// 自选商品套餐 套餐中的附属商品，消费者可以通过复选框的方式，有选择的购买
                /// </summary>
                public bool Fixed { get; set; }

                /// <summary>
                /// 搭配组合套餐附属商品
                /// </summary>
                public IList<Sub> Subs { get; set; }


                public Package()
                {
                    Subs = new List<Sub>();
                }

                /// <summary>
                /// 搭配组合套餐附属商品
                /// </summary>
                public class Sub : Sku
                {
                    /// <summary>
                    /// SkuId
                    /// </summary>

                    public int SkuId { get; set; }

                    /// <summary>
                    /// 是否为Sku
                    /// </summary>
                    public bool IsSku => SkuId > 0 && ItemId > 0;
                }

            }

            #endregion

        }

        #endregion
    }
    #endregion


    #region 赠品
    /// <summary>
    /// 赠品
    /// </summary>
    public class Gift
    {
        /// <summary>
        /// 商品Sku Id
        /// </summary>
        public int SkuId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 商品Sku属性
        /// </summary>
        public string PropText { get; set; }

    }
    #endregion

    #region 店铺优惠券
    /// <summary>
    /// 店铺优惠券 http://bbs.taobao.com/catalog/thread/16543510-264269834.htm
    /// </summary>
    public class Coupon
    {
        /// <summary>
        /// 适用于全店铺
        /// </summary>
        public bool Global { get; set; }

        /// <summary>
        /// 面额 3 5 10 20 50 100 200
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 适用范围 pc wap app 通用
        /// </summary>
        public Platform Platform { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime StartedOn { get; set; }

        /// <summary>
        /// 失效时间 失效时间不应早于生效时间及活动结束时间
        /// </summary>
        public DateTime StoppedOn { get; set; }

        /// <summary>
        /// 使用条件 不限/订单满x元
        /// </summary>
        public bool Limit { get; set; }

        /// <summary>
        /// 使用条件值 订单满x元
        /// </summary>
        public decimal LimitVal { get; set; }

    }
    #endregion

    #region 折扣
    /// <summary>
    /// 订单使用到的优惠 包括单品
    /// </summary>
    public class Discount
    {
        /// <summary>
        /// 促销Id
        /// </summary>
        public int PromotionId { get; set; }

        /// <summary>
        /// 促销名称
        /// </summary>
        public string PromotionName { get; set; }

        /// <summary>
        /// 促销类型
        /// </summary>
        public Promotion.Type PromotionType { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 单品优惠过了该值必须为true 防止重复优惠
        /// 因为单品的优惠 直接作用到了价格上
        /// </summary>        
        public bool Pass { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public Discount(Promotion.Type type, int id, string name)
        {
            PromotionId = id;
            PromotionName = name;
            PromotionType = type;
        }
    }

    #endregion

    #region 支付配送

    #endregion


    #region 操作
    /// <summary>
    /// 购物车操作
    /// </summary>
    public enum Operate : byte
    {
        /// <summary>
        /// 增加
        /// </summary>
        Increase = 0,

        /// <summary>
        /// 减少
        /// </summary>
        Decrease = 1,

        /// <summary>
        /// 移除
        /// </summary>
        Remove = 2
    }
    #endregion

    #region 选择
    /// <summary>
    /// 选择
    /// </summary>
    public enum Select : byte
    {
        /// <summary>
        /// 全选
        /// </summary>
        All = 0,

        /// <summary>
        /// 选择指定购物蓝
        /// </summary>
        Basket = 1,

        /// <summary>
        /// 选择指定商品Sku
        /// </summary>
        Sku = 2,

        /// <summary>
        /// 选择指定商品
        /// </summary>
        Item = 3,

        /// <summary>
        /// 选择指定搭配组合套餐
        /// </summary>
        Package = 4
    }
    #endregion

    /// <summary>
    /// 已选数据
    /// </summary>
    public class Summary
    {
        /// <summary>
        /// 已选件数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 已选商品金额总计 不含运费
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 优惠
        /// </summary>
        public decimal Discount { get; set; }
    }

    /*
    #region 购物车类型
    /// <summary>
    /// 购物车类型
    /// </summary>
    public enum Type : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 货到付款
        /// </summary>
        COD = 2,

        /// <summary>
        /// 海外的
        /// </summary>
        Oversea = 4,

        /// <summary>
        /// 全部
        /// </summary>
        All = Normal | COD | Oversea

    }
    #endregion
    */

    public class SubmitParam
    {
        public int[] Skus { get; set; }
        public int[] Items { get; set; }
        public int[] Packages { get; set; }
        public string IpAddress { get; set; }
        public int UserAddressId { get; set; }
        public PaymentType PaymentType { get; set; }
        //public Invoice Invoice { get; set; }
        public IDictionary<int, string> Messages { get; set; }

    }


}