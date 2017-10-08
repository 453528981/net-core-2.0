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
        /// 搭配组合套餐 
        /// http://bbs.taobao.com/catalog/thread/16543510-264271296.htm
        /// created on 2016-05-22 01:06:49
        ///</summary>
        [ProtoContract]
        public class Package : IEntity<int>
        {
            ///<summary>
            /// Id
            ///</summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// 套餐名称
            ///</summary>
            [ProtoMember(2)]
            public string Name { get; set; }

            ///<summary>
            /// 固定组合套餐 商品打包成套餐销售 消费者打包购买 自选商品套餐 套餐中的附属商品 消费者可以通过复选框的方式有选择的购买
            ///</summary>
            [ProtoMember(3)]
            public bool Fixed { get; set; }

            ///<summary>
            /// 套餐简介 摘要 将出现在主商品详情描述页面 衣介绍套餐的卖点
            ///</summary>
            [ProtoMember(4)]
            public string Summary { get; set; }

            ///<summary>
            /// 限购开始时间
            ///</summary>
            [ProtoMember(5)]
            public DateTime StartedOn { get; set; }

            ///<summary>
            /// 限购结束时间
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
            /// 主商品Id
            ///</summary>
            [ProtoMember(9)]
            public int ItemId { get; set; }

            ///<summary>
            /// 主商品名称
            ///</summary>
            [ProtoMember(10)]
            public string ItemName { get; set; }

            ///<summary>
            /// 主商品搭配价格 0为默认如果不设置搭配价 则执行在售价(适用于有多个不同Sku 如果没有sku则可设置一个搭配价格)
            ///</summary>
            [ProtoMember(11)]
            public decimal ItemPrice { get; set; }

            ///<summary>
            /// 主商品搭配图
            ///</summary>
            [ProtoMember(12)]
            public string ItemPictrue { get; set; }

            ///<summary>
            /// 用户参与活动限制类型 0无限制 1限制该活动总的参与次数 2限制该活动每个用户可参与次数
            ///</summary>
            [ProtoMember(13)]
            public LimitType LimitType { get; set; }

            ///<summary>
            /// 用户参与活动限制值 LimitType不为0时有效
            ///</summary>
            [ProtoMember(14)]
            public int LimitValue { get; set; }

            ///<summary>
            /// 卖家Id
            ///</summary>
            [ProtoMember(15)]
            public int SellerId { get; set; }

            ///<summary>
            /// 卖家名称
            ///</summary>
            [ProtoMember(16)]
            public string SellerName { get; set; }

            ///<summary>
            /// 状态 1为可用 0为不可用
            ///</summary>
            [ProtoMember(17)]
            public bool Status { get; set; }

            ///<summary>
            /// 创建时间
            ///</summary>
            [ProtoMember(18)]
            public DateTime CreatedOn { get; set; }

            ///<summary>
            /// 最后一次编辑者
            ///</summary>
            [ProtoMember(19)]
            public string ModifiedBy { get; set; }

            ///<summary>
            /// 最后一次编辑时间
            ///</summary>
            [ProtoMember(20)]
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
            /// Items
            /// </summary>
            public virtual IList<Item> Items { get; set; }

            /// <summary>
            /// 判断活动在指定平台是否有效            
            /// </summary>
            /// <param name="platform">适用平台</param>
            /// <returns></returns>
            public bool IsValid(Platform platform)
            {
                var now = DateTime.Now;
                var available=((Platform& platform) == platform);//检查当前促销是否适用于给定平台
                return Status && StartedOn < now && now < StoppedOn && available && Items.Any(x => x.Status);
            }

            ///<summary>
            /// 搭配组合套餐附属商品
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
                /// 搭配组合套餐Id
                ///</summary>
                [ProtoMember(2)]
                public int ParentId { get; set; }

                ///<summary>
                /// 主商品Id
                ///</summary>
                [ProtoMember(3)]
                public int MainId { get; set; }

                ///<summary>
                /// 附属商品ItemId(无sku)
                ///</summary>
                [ProtoMember(4)]
                public int ItemId { get; set; }

                ///<summary>
                /// 附属商品SkuId
                ///</summary>
                [ProtoMember(5)]
                public int SkuId { get; set; }

                ///<summary>
                /// 附属商品名称
                ///</summary>
                [ProtoMember(6)]
                public string Name { get; set; }

                ///<summary>
                /// 附属商品价格 0为默认如果不设置搭配价 则执行在售价
                ///</summary>
                [ProtoMember(7)]
                public decimal Price { get; set; }

                ///<summary>
                /// 附属商品图片
                ///</summary>
                [ProtoMember(8)]
                public string Picture { get; set; }

                ///<summary>
                /// 默认勾选
                ///</summary>
                [ProtoMember(9)]
                public bool Selected { get; set; }

                ///<summary>
                /// 排序 从小到大
                ///</summary>
                [ProtoMember(10)]
                public int Priority { get; set; }

                ///<summary>
                /// 卖家Id
                ///</summary>
                [ProtoMember(11)]
                public int SellerId { get; set; }

                ///<summary>
                /// 状态 1为可用 0为不可用
                ///</summary>
                [ProtoMember(12)]
                public bool Status { get; set; }

                ///<summary>
                /// 创建时间
                ///</summary>
                [ProtoMember(13)]
                public DateTime CreatedOn { get; set; }

                ///<summary>
                /// 最后一次编辑者
                ///</summary>
                [ProtoMember(14)]
                public string ModifiedBy { get; set; }

                ///<summary>
                /// 最后一次编辑时间
                ///</summary>
                [ProtoMember(15)]
                public DateTime ModifiedOn { get; set; }

            }

        }
    }
}

