using System;
using ProtoBuf;
using Newtonsoft.Json;

namespace Ayatta.Domain
{
    ///<summary>
    /// OrderItem
    ///</summary>
    [ProtoContract]
    public class OrderItem : IEntity<string>
    {

        ///<summary>
        /// 订单明细Id
        ///</summary>
        [ProtoMember(1)]
        public string Id { get; set; }

        ///<summary>
        /// 订单Id
        ///</summary>
        [ProtoMember(2)]
        public string OrderId { get; set; }

        ///<summary>
        /// SpuId
        ///</summary>
        [ProtoMember(3)]
        public int SpuId { get; set; }

        ///<summary>
        /// ItemId
        ///</summary>
        [ProtoMember(4)]
        public int ItemId { get; set; }

        ///<summary>
        /// SkuId
        ///</summary>
        [ProtoMember(5)]
        public int SkuId { get; set; }

        ///<summary>
        /// 根类目id
        ///</summary>
        [ProtoMember(6)]
        public int CatgRId { get; set; }

        ///<summary>
        /// 中间类目id
        ///</summary>
        [ProtoMember(7)]
        public string CatgMId { get; set; }

        ///<summary>
        /// 最小类目id
        ///</summary>
        [ProtoMember(8)]
        public int CatgId { get; set; }

        ///<summary>
        /// 套餐Id
        ///</summary>
        [ProtoMember(9)]
        public int PackageId { get; set; }

        ///<summary>
        /// 套餐名
        ///</summary>
        [ProtoMember(10)]
        public string PackageName { get; set; }

        ///<summary>
        /// 商家设置的外部id
        ///</summary>
        [ProtoMember(11)]
        public string Code { get; set; }

        ///<summary>
        /// 商品名称
        ///</summary>
        [ProtoMember(12)]
        public string Name { get; set; }

        ///<summary>
        /// 成交时真实单价 精确到2位小数 单位 元
        ///</summary>
        [ProtoMember(13)]
        public decimal Price { get; set; }

        ///<summary>
        /// 交易页展示单价 精确到2位小数 单位 元
        ///</summary>
        [ProtoMember(14)]
        public decimal PriceShow { get; set; }

        ///<summary>
        /// 数量
        ///</summary>
        [ProtoMember(15)]
        public int Quantity { get; set; }

        ///<summary>
        /// 关税税费
        ///</summary>
        [ProtoMember(16)]
        public decimal Tax { get; set; }

        ///<summary>
        /// 卖家手工调整金额
        ///</summary>
        [ProtoMember(17)]
        public decimal Adjust { get; set; }

        ///<summary>
        /// 订单优惠金额 精确到2位小数 单位元
        ///</summary>
        [ProtoMember(18)]
        public decimal Discount { get; set; }

        ///<summary>
        /// 商品金额小计 精确到2位小数 单位元
        ///</summary>
        [ProtoMember(19)]
        public decimal Total { get; set; }

        ///<summary>
        /// 关税税率
        ///</summary>
        [ProtoMember(20)]
        public decimal TaxRate { get; set; }

        ///<summary>
        /// 图片
        ///</summary>
        [ProtoMember(21)]
        public string Picture { get; set; }

        ///<summary>
        /// 商品销售属性
        ///</summary>
        [JsonIgnore]
        [ProtoMember(22)]
        public string PropText { get; set; }

        ///<summary>
        /// 是否为赠品
        ///</summary>
        [ProtoMember(23)]
        public bool IsGift { get; set; }

        ///<summary>
        /// 是否为虚拟物品
        ///</summary>
        [ProtoMember(24)]
        public bool IsVirtual { get; set; }

        ///<summary>
        /// 是否是服务项目
        ///</summary>
        [ProtoMember(25)]
        public bool IsService { get; set; }

        ///<summary>
        /// 优惠详情json格式
        ///</summary>
        [ProtoMember(26)]
        public string PromotionData { get; set; }

        ///<summary>
        /// 超时时间
        ///</summary>
        [ProtoMember(27)]
        public DateTime ExpiredOn { get; set; }

        ///<summary>
        /// 子订单发货时间，当卖家对订单进行了多次发货，子订单的发货时间和主订单的发货时间可能不一样了，那么就需要以子订单的时间为准。没有进行多次发货的订单，主订单的发货时间和子订单的发货时间都一样
        ///</summary>
        [ProtoMember(28)]
        public DateTime? ConsignedOn { get; set; }

        ///<summary>
        /// 子订单的交易结束时间 说明：子订单有单独的结束时间，与主订单的结束时间可能有所不同，在有退款发起的时候或者是主订单分阶段付款的时候，子订单的结束时间会早于主订单的结束时间
        ///</summary>
        [ProtoMember(29)]
        public DateTime? FinishedOn { get; set; }

        ///<summary>
        /// 子订单所在包裹的运单号
        ///</summary>
        [ProtoMember(30)]
        public string LogisticsNo { get; set; }

        ///<summary>
        /// 子订单发货的物流公司名称
        ///</summary>
        [ProtoMember(31)]
        public string LogisticsName { get; set; }

        ///<summary>
        /// 退/换货Id
        ///</summary>
        [ProtoMember(32)]
        public string ReturnId { get; set; }

        ///<summary>
        /// 退/换货状态
        ///</summary>
        [ProtoMember(33)]
        public byte ReturnStatus { get; set; }

        ///<summary>
        /// 退款Id
        ///</summary>
        [ProtoMember(34)]
        public string RefundId { get; set; }

        ///<summary>
        /// 退款状态
        ///</summary>
        [ProtoMember(35)]
        public byte RefundStatus { get; set; }

        ///<summary>
        /// 买家Id
        ///</summary>
        [ProtoMember(36)]
        public int BuyerId { get; set; }

        ///<summary>
        /// 买家用户名
        ///</summary>
        [ProtoMember(37)]
        public string BuyerName { get; set; }

        ///<summary>
        /// 买家是否已评价 可选值:true(已评价),false(未评价) 如买家只评价未打分，此字段仍返回false
        ///</summary>
        [ProtoMember(38)]
        public bool BuyerRated { get; set; }

        ///<summary>
        /// 卖家Id
        ///</summary>
        [ProtoMember(39)]
        public int SellerId { get; set; }

        ///<summary>
        /// 卖家用户名
        ///</summary>
        [ProtoMember(40)]
        public string SellerName { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        [ProtoMember(41)]
        public string Extra { get; set; }

        ///<summary>
        /// 子订单状态
        ///</summary>
        [ProtoMember(42)]
        public OrderStatus Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(43)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(44)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(45)]
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public virtual string[] PropTexts
        {
            get
            {
                if (!string.IsNullOrEmpty(PropText))
                {
                    return PropText.Split(';');
                }
                return new string[0];
            }
        }
        public bool IsSku => (SkuId > 0 && ItemId > 0);

    }
}