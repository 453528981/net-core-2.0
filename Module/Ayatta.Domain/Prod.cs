using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    public static partial class Prod
    {
        public struct Prop
        {
            public int PId;
            public string PName;
            public int VId;
            public string VName;
            public string Extra;
        }
        
        /// <summary>
        ///  商品状态
        /// </summary>
        public enum Status : byte
        {
            /// <summary>
            /// 上线出售中
            /// </summary>
            Online = 0,

            /// <summary>
            /// 下线库中
            /// </summary>
            Offline = 1,

            /// <summary>
            /// 被商家删除的
            /// </summary>
            Deleted = 2,

            /// <summary>
            /// 被系统隔离的 违规 违反广告法 包含敏感词等
            /// </summary>
            Isolated = 3,

            /// <summary>
            /// 被系统禁用的 被品牌商投诉侵权等
            /// </summary>
            Forbidden = 4
        }

        [ProtoContract]
        public class Current : IEntity<int>
        {
            ///<summary>
            /// Id
            ///</summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// 商品库存
            ///</summary>
            [ProtoMember(2)]
            public int Stock { get; set; }

            ///<summary>
            /// 商品价格
            ///</summary>
            [ProtoMember(3)]
            public decimal Price { get; set; }

            ///<summary>
            /// app商品价格
            ///</summary>
            [ProtoMember(4)]
            public decimal AppPrice { get; set; }

            ///<summary>
            /// 商品建议零售价格
            ///</summary>
            [ProtoMember(5)]
            public decimal RetailPrice { get; set; }

            /// <summary>
            /// 活动特价
            /// </summary>
            [ProtoMember(5)]
            public decimal SpecialPrice { get; set; }

            ///<summary>
            /// 商家Id
            ///</summary>
            [ProtoMember(6)]
            public int SellerId { get; set; }

            /// <summary>
            /// Sku列表
            /// </summary>
            [ProtoMember(100)]
            public virtual IList<Sku> Skus { get; set; }

            /// <summary>
            /// 获取最低价
            /// </summary>
            /// <param name="platform">平台</param>
            /// <returns></returns>
            public decimal GetDisplayPrice(Platform platform)
            {
                if (platform == Platform.App)
                {
                    if (SpecialPrice > 0 && SpecialPrice < AppPrice)
                    {
                        return AppPrice;
                    }
                    return Price;
                }
                if (SpecialPrice > 0 && SpecialPrice < Price)
                {
                    return SpecialPrice;
                }
                return Price;
            }

            [ProtoContract]
            public class Sku : IEntity<int>
            {
                ///<summary>
                /// Id
                ///</summary>
                [ProtoMember(1)]
                public int Id { get; set; }

                ///<summary>
                /// 商品库存
                ///</summary>
                [ProtoMember(2)]
                public int Stock { get; set; }

                ///<summary>
                /// 商品价格
                ///</summary>
                [ProtoMember(3)]
                public decimal Price { get; set; }

                ///<summary>
                /// app商品价格
                ///</summary>
                [ProtoMember(4)]
                public decimal AppPrice { get; set; }

                ///<summary>
                /// 商品建议零售价格
                ///</summary>
                [ProtoMember(5)]
                public decimal RetailPrice { get; set; }

                /// <summary>
                /// 活动特价
                /// </summary>
                [ProtoMember(5)]
                public decimal SpecialPrice { get; set; }

                /// <summary>
                /// 获取最低价
                /// </summary>
                /// <param name="platform">平台</param>
                /// <returns></returns>
                public decimal GetDisplayPrice(Platform platform)
                {
                    if (platform == Platform.App)
                    {
                        if (SpecialPrice > 0 && SpecialPrice < AppPrice)
                        {
                            return AppPrice;
                        }
                        return Price;
                    }
                    if (SpecialPrice > 0 && SpecialPrice < Price)
                    {
                        return SpecialPrice;
                    }
                    return Price;
                }
            }

            public class Special : IEntity<int>
            {
                ///<summary>
                /// Id
                ///</summary>
                [ProtoMember(1)]
                public int Id { get; set; }

                ///<summary>
                /// 商品库存
                ///</summary>
                [ProtoMember(2)]
                public Platform Platform { get; set; }

                ///<summary>
                /// 商品Id
                ///</summary>
                [ProtoMember(3)]
                public int ItemId { get; set; }

                ///<summary>
                /// 统一设置优惠(商品维度)
                ///</summary>
                [ProtoMember(4)]
                public bool Global { get; set; }

                ///<summary>
                /// 统一设置优惠值(商品维度)
                ///</summary>
                [ProtoMember(5)]
                public decimal Value { get; set; }

                ///<summary>
                /// 对Sku设置的优惠信息 Json格式
                ///</summary>
                [JsonIgnore]
                [ProtoMember(6)]
                public string SkuJson { get; set; }

                /// <summary>
                /// 对Sku设置的优惠
                /// </summary>
                public virtual IDictionary<int, decimal> Skus
                {
                    get
                    {
                        if (Global || string.IsNullOrEmpty(SkuJson)) return null;

                        return JsonConvert.DeserializeObject<Dictionary<int, decimal>>(SkuJson);
                    }
                }
            }
        }
    }
}