using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    /// <summary>
    /// 促销 http://bbs.taobao.com/catalog/thread/16543510-264265243.htm
    /// </summary>
    public partial class Promotion
    {

        ///<summary>
        /// 特价 
        /// http://bbs.taobao.com/catalog/thread/16543510-264264853.htm
        ///<summary>
        [ProtoContract]
        public class SpecialPrice : IEntity<int>
        {
            ///<summary>
            /// Id
            ///</summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// A打折  B减价  C促销价 活动创建后优惠方式将不能修改
            ///</summary>
            [ProtoMember(2)]
            public SpecialPriceType Type { get; set; }

            ///<summary>
            /// 活动名称
            ///</summary>
            [ProtoMember(3)]
            public string Name { get; set; }

            ///<summary>
            /// 优惠标题
            ///</summary>
            [ProtoMember(4)]
            public string Title { get; set; }

            ///<summary>
            /// 开始时间
            ///</summary>
            [ProtoMember(5)]
            public DateTime StartedOn { get; set; }

            ///<summary>
            /// 结束时间
            ///</summary>
            [ProtoMember(6)]
            public DateTime StoppedOn { get; set; }

            ///<summary>
            /// 活动适用平台 0为None 1为适用平于pc 2为适用平于wap 4为适用平于app
            ///</summary>
            [ProtoMember(7)]
            public Platform Platform { get; set; }

            ///<summary>
            /// 限定媒体Id 空为无限定 如需限定部分媒体 使用","分隔
            ///</summary>
            [ProtoMember(8)]
            public string MediaScope { get; set; }

            ///<summary>
            /// 免运费
            ///</summary>
            [ProtoMember(9)]
            public bool FreightFree { get; set; }

            ///<summary>
            /// 免运费排除在外的地区(以,分隔)
            ///</summary>
            [ProtoMember(10)]
            public string FreightFreeExclude { get; set; }

            ///<summary>
            /// 卖家Id
            ///</summary>
            [ProtoMember(11)]
            public int SellerId { get; set; }

            ///<summary>
            /// 卖家名称
            ///</summary>
            [ProtoMember(12)]
            public string SellerName { get; set; }

            ///<summary>
            /// 状态 1为可用 0为不可用
            ///</summary>
            [ProtoMember(13)]
            public bool Status { get; set; }

            ///<summary>
            /// 创建时间
            ///</summary>
            [ProtoMember(14)]
            public DateTime CreatedOn { get; set; }

            ///<summary>
            /// 最后一次编辑者
            ///</summary>
            [ProtoMember(15)]
            public string ModifiedBy { get; set; }

            ///<summary>
            /// 最后一次编辑时间
            ///</summary>
            [ProtoMember(16)]
            public DateTime ModifiedOn { get; set; }


            /// <summary>
            /// 活动限定媒体
            /// </summary>
            public IList<int> Medias
            {
                get
                {
                    if (!string.IsNullOrEmpty(MediaScope))
                    {
                        return MediaScope.Split(',').Select(x => int.Parse(x)).ToArray();
                    }
                    return new List<int>(0);
                }
            }

            /// <summary>
            /// 特价活动商品
            /// </summary>
            [ProtoMember(16)]
            public virtual IList<Item> Items { get; set; }

            /// <summary>
            /// 判断活动在指定平台是否有效            
            /// </summary>
            /// <param name="platform">适用平台</param>
            /// <returns></returns>
            public bool IsValid(Platform platform)
            {
                var now = DateTime.Now;
                var available = ((Platform & platform) == platform);//检查当前促销是否适用于给定平台
                return Status && StartedOn < now && now < StoppedOn && available && Items.Any(x => x.Status);
            }

            /// <summary>
            /// 判断特价是否包含指定的商品
            /// </summary>
            /// <param name="Platform">适用平台</param>
            /// <param name="itemId">商品ItemId</param>
            /// <param name="skuId">商品SkuId</param>
            /// <returns></returns>
            public Magic<bool, decimal> Contains(int itemId, int? skuId = null)
            {
                var item = Items.FirstOrDefault(x => x.ItemId == itemId);
                if (item != null)
                {
                    if (item.Global)
                    {
                        return new Magic<bool, decimal>(true, item.Value);
                    }
                    if (skuId.HasValue && item.Skus != null)
                    {
                        if (item.Skus.ContainsKey(skuId.Value))
                        {
                            return new Magic<bool, decimal>(true, item.Value);
                        }
                        return new Magic<bool, decimal>(false);
                    }
                }
                return new Magic<bool, decimal>(false);
            }

            ///<summary>
            /// 特价
            ///</summary>
            [ProtoContract]
            public class Item : IEntity<int>
            {

                ///<summary>
                /// Id
                ///</summary>
                [ProtoMember(1)]
                public int Id { get; set; }

                ///<summary>
                /// 特价Id
                ///</summary>
                [ProtoMember(2)]
                public int ParentId { get; set; }

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
                /// 用户参与活动限制类型 0无限制 1限制该活动总的参与次数 2限制该活动每个用户可参与次数
                ///</summary>
                [ProtoMember(6)]
                public LimitType LimitType { get; set; }

                ///<summary>
                /// 用户参与活动限制值 LimitType不为0时有效
                ///</summary>
                [ProtoMember(7)]
                public int LimitValue { get; set; }

                ///<summary>
                /// 对Sku设置的优惠信息 Json格式
                ///</summary>
                [ProtoMember(8)]
                public string SkuData { get; set; }

                ///<summary>
                /// 卖家Id
                ///</summary>
                [ProtoMember(9)]
                public int SellerId { get; set; }

                ///<summary>
                /// 状态 1为可用 0为不可用
                ///</summary>
                [ProtoMember(10)]
                public bool Status { get; set; }

                ///<summary>
                /// 创建时间
                ///</summary>
                [ProtoMember(11)]
                public DateTime CreatedOn { get; set; }

                ///<summary>
                /// 最后一次编辑者
                ///</summary>
                [ProtoMember(12)]
                public string ModifiedBy { get; set; }

                ///<summary>
                /// 最后一次编辑时间
                ///</summary>
                [ProtoMember(13)]
                public DateTime ModifiedOn { get; set; }



                /// <summary>
                /// 对Sku设置的优惠
                /// </summary>
                public virtual IDictionary<int, decimal> Skus
                {
                    get
                    {
                        if (Global || string.IsNullOrEmpty(SkuData)) return null;

                        return JsonConvert.DeserializeObject<Dictionary<int, decimal>>(SkuData);
                    }
                }
            }
        }
    }
}

