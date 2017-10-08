using System;
using ProtoBuf;
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Cart
{
    /// <summary>
    /// 购物车缓存数据
    /// </summary>
    [ProtoContract]
    internal sealed class CacheData
    {
        /// <summary>
        /// UserId
        /// </summary>
        [ProtoMember(1)]
        public int UserId { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [ProtoMember(2)]
        public string UserName { get; set; }

        /// <summary>
        /// UserGrade
        /// </summary>
        [ProtoMember(3)]
        public UserGrade UserGrade { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [ProtoMember(4)]
        public UserAddress UserAddress { get; set; }

        /// <summary>
        /// 按卖家拆分的购物蓝
        /// </summary>
        [ProtoMember(5)]
        public IDictionary<int, Basket> Baskets { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [ProtoMember(6)]

        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 发票信息
        /// </summary>
        [ProtoMember(7)]
        public Invoice Invoice { get; set; }

        /// <summary>
        /// 报关所需信息
        /// </summary>
        [ProtoMember(8)]
        public Identity Identity { get; set; }

        /// <summary>
        /// 时间戳 最后一次操作时间
        /// </summary>
        [ProtoMember(9)]
        public DateTime Timestamp { get; set; }

        #region Constructor
        public CacheData()
        {
            Baskets = new Dictionary<int, Basket>();
        }

        #endregion

        #region
        internal Basket GetBasket(int id)
        {
            return Baskets.ContainsKey(id) ? Baskets[id] : null;
        }

        internal void AddBasket(int id, string name, string sellerName)
        {
            if (Baskets.ContainsKey(id)) return;
            var basket = new Basket
            {
                Name = name,
                SellerName = sellerName
            };
            Baskets.Add(id, basket);
        }

        internal void Merge(CacheData data)
        {
            if (data != null)
            {
                foreach (var basket in data.Baskets)
                {
                    AddBasket(basket.Key, basket.Value.Name, basket.Value.Name);
                    
                    foreach (var sku in basket.Value.Skus)
                    {
                        Baskets[basket.Key].AddSku(sku.Key, sku.Value.ItemId, sku.Value.Quantity);
                    }
                    foreach (var item in basket.Value.Items)
                    {
                        Baskets[basket.Key].AddItem(item.Key, item.Value.Quantity);
                    }
                }               
            }
        }
        #endregion

        #region 购物篮
        [ProtoContract]
        public sealed class Basket
        {
            /// <summary>
            /// 卖家店铺名称
            /// </summary>
            [ProtoMember(2)]
            public string Name { get; set; }

            /// <summary>
            /// 卖家名称
            /// </summary>
            [ProtoMember(3)]
            public string SellerName { get; set; }

            /// <summary>
            /// 商品Sku
            /// </summary>
            [ProtoMember(4)]
            public IDictionary<int, Sku> Skus { get; set; }

            /// <summary>
            /// 商品
            /// </summary>
            [ProtoMember(5)]
            public IDictionary<int, Item> Items { get; set; }

            /// <summary>
            /// 套餐
            /// </summary>
            [ProtoMember(6)]
            public IDictionary<int, Package> Packages { get; set; }


            #region Constructor
            public Basket()
            {
                Skus = new Dictionary<int, Sku>();
                Items = new Dictionary<int, Item>();
                Packages = new Dictionary<int, Package>();
            }
            #endregion

            #region Internal Method

            /// <summary>
            /// 添加商品Sku
            /// </summary>
            /// <param name="skuId">商品Sku id</param>
            /// <param name="itemId">商品 id</param>
            /// <param name="quantity">添加数量</param>
            internal void AddSku(int skuId, int itemId, int quantity = 1)
            {
                if (Skus.ContainsKey(skuId))
                {
                    Skus[skuId].Quantity += quantity;
                }
                else
                {
                    Skus[skuId] = new Sku { Quantity = quantity, ItemId = itemId };
                }
            }

            /// <summary>
            /// 减少商品Sku
            /// </summary>
            /// <param name="skuId">商品Sku id</param>
            /// <param name="quantity">减少数量</param>
            internal void SubtractSku(int skuId, int quantity = 1)
            {
                if (!Skus.ContainsKey(skuId)) return;
                var q = Skus[skuId].Quantity;
                var v = q - quantity;
                if (v > 0)
                {
                    Skus[skuId].Quantity = v;
                }
                else
                {
                    Skus.Remove(skuId);
                }
            }

            /// <summary>
            /// 设置商品Sku数量
            /// </summary>
            /// <param name="skuId">商品Sku id</param>
            /// <param name="itemId">商品 id</param>
            /// <param name="quantity">设置数量</param>
            internal void SetSkuQuantity(int skuId, int itemId, int quantity = 1)
            {
                if (quantity <= 1)
                {
                    RemoveSku(skuId);
                    return;
                }
                if (Skus.ContainsKey(skuId))
                {
                    Skus[skuId].Quantity = quantity;
                }
                else
                {
                    Skus[skuId] = new Sku { Quantity = quantity, ItemId = itemId };
                }
            }

            /// <summary>
            /// 移除商品Sku
            /// </summary>
            /// <param name="skuId">商品Sku id</param>
            internal void RemoveSku(int skuId)
            {
                if (Skus.ContainsKey(skuId))
                {
                    Skus.Remove(skuId);
                }
            }


            internal void AddItem(int itemId, int quantity = 1)
            {
                if (Items.ContainsKey(itemId))
                {
                    Items[itemId].Quantity += quantity;
                }
                else
                {
                    Items[itemId] = new Item { Quantity = quantity };
                }
            }

            internal void SubtractItem(int itemId, int quantity = 1)
            {
                if (!Items.ContainsKey(itemId)) return;
                var q = Items[itemId].Quantity;
                var v = q - quantity;
                if (v > 0)
                {
                    Items[itemId].Quantity = v;
                }
                else
                {
                    Items.Remove(itemId);
                }
            }

            internal void SetItemQuantity(int itemId, int quantity = 1)
            {
                if (quantity <= 1)
                {
                    RemoveItem(itemId);
                    return;
                }
                if (Items.ContainsKey(itemId))
                {
                    Items[itemId].Quantity = quantity;
                }
                else
                {
                    Items[itemId] = new Item { Quantity = quantity };
                }
            }

            internal void RemoveItem(int itemId)
            {
                if (Items.ContainsKey(itemId))
                {
                    Items.Remove(itemId);
                }
            }

            internal void AddPackage(int packageId, IList<int> subs, int quantity = 1)
            {
                if (Packages.ContainsKey(packageId))
                {
                    Packages[packageId].Quantity += quantity;
                }
                else
                {
                    Packages[packageId] = new Package { Quantity = quantity, Subs = subs };
                }
            }

            internal void SubtractPackage(int packageId, int quantity = 1)
            {
                if (!Packages.ContainsKey(packageId)) return;
                var q = Packages[packageId].Quantity;
                var v = q - quantity;
                if (v > 0)
                {
                    Packages[packageId].Quantity = v;
                }
                else
                {
                    Packages.Remove(packageId);
                }
            }

            internal void RemovePackage(int packageId)
            {
                if (Packages.ContainsKey(packageId))
                {
                    Packages.Remove(packageId);
                }
            }

            #endregion

            #region

            /// <summary>
            /// 商品
            /// </summary>
            [ProtoContract]
            public class Item
            {
                /// <summary>
                /// 数量
                /// </summary>
                [ProtoMember(1)]
                public int Quantity { get; set; }

                /// <summary>
                /// 是否选中
                /// </summary>
                [ProtoMember(2)]
                public bool Selected { get; set; }

            }

            /// <summary>
            /// 商品Sku
            /// </summary>
            [ProtoContract]
            public class Sku
            {
                /// <summary>
                /// 商品ItemId
                /// </summary>
                [ProtoMember(1)]
                public int ItemId { get; set; }

                /// <summary>
                /// 数量
                /// </summary>
                [ProtoMember(2)]
                public int Quantity { get; set; }

                /// <summary>
                /// 是否选中
                /// </summary>
                [ProtoMember(3)]
                public bool Selected { get; set; }

            }


            /// <summary>
            /// 搭配组合套餐
            /// </summary>
            [ProtoContract]
            public class Package
            {
                /// <summary>
                /// 数量
                /// </summary>
                [ProtoMember(1)]
                public int Quantity { get; set; }

                /// <summary>
                /// 附属商品Id集合
                /// </summary>
                [ProtoMember(2)]
                public IList<int> Subs { get; set; }

                /// <summary>
                /// 是否选中
                /// </summary>
                [ProtoMember(3)]
                public bool Selected { get; set; }

                #region Constructor
                public Package()
                {
                    Subs = new List<int>();
                }
                #endregion

            }
            #endregion
        }
        #endregion

    }

    #region

    /// <summary>
    /// 发票
    /// </summary>
    [ProtoContract]
    public class Invoice
    {
        /// <summary>
        /// 0为不需要 1纸质发票 2为电子发票
        /// </summary>
        [ProtoMember(1)]
        public byte Type { get; set; }

        /// <summary>
        /// 是否为个人
        /// </summary>
        [ProtoMember(2)]
        public bool Personal { get; set; }

        /// <summary>
        /// 发票抬头（Personal为false时该项必填）
        /// </summary>
        [ProtoMember(3)]
        public string Title { get; set; }

        /// <summary>
        ///  内容
        /// </summary>
        [ProtoMember(4)]
        public string Content { get; set; }
    }

    #endregion

    #region 报关所需信息
    /// <summary>
    /// 报关所需信息
    /// </summary>
    [ProtoContract]
    public struct Identity
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        [ProtoMember(1)]
        public string Code;

        /// <summary>
        /// 真实姓名
        /// </summary>
        [ProtoMember(2)]
        public string Name;

        /// <summary>
        /// 是否通过验证
        /// </summary>
        [ProtoMember(3)]
        public bool IsAuthenticated;
    }
    #endregion

}