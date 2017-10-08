using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    /// <summary>
    /// 订单
    /// </summary>

    public partial class Order : IEntity<string>
    {
        #region Properties

        ///<summary>
        /// 订单Id
        ///</summary>
        [ProtoMember(1)]
        public string Id { get; set; }

        ///<summary>
        /// 订单类型:0网购订单 1竞购订单 2竞购补差订单 3积分兑换订单
        ///</summary>
        [ProtoMember(2)]
        public OrderType Type { get; set; }

        ///<summary>
        /// 商品总数量
        ///</summary>
        [ProtoMember(3)]
        public int Quantity { get; set; }

        ///<summary>
        /// 订单小计(商品总金额)
        ///</summary>
        [ProtoMember(4)]
        public decimal SubTotal { get; set; }

        ///<summary>
        /// 运费
        ///</summary>
        [ProtoMember(5)]
        public decimal Freight { get; set; }

        ///<summary>
        /// 关税税费
        ///</summary>
        [ProtoMember(6)]
        public decimal Tax { get; set; }

        ///<summary>
        /// 订单优惠总金额(使用积分抵扣 优惠券 店铺优惠等)
        ///</summary>
        [ProtoMember(7)]
        public decimal Discount { get; set; }

        ///<summary>
        /// 订单总金额(需要支付的总金额[SubTotal+Postage-Discount])
        ///</summary>
        [ProtoMember(8)]
        public decimal Total { get; set; }

        ///<summary>
        /// 已支付金额(一个订单可能分多次支付)
        ///</summary>
        [ProtoMember(9)]
        public decimal Paid { get; set; }

        ///<summary>
        /// 支付流水号
        ///</summary>
        [ProtoMember(10)]
        public string PayId { get; set; }

        ///<summary>
        /// 支付日期(最后一次支付完成整个订单的时间)
        ///</summary>
        [ProtoMember(11)]
        public DateTime? PaidOn { get; set; }

        ///<summary>
        /// 使用积分
        ///</summary>
        [ProtoMember(12)]
        public int PointUse { get; set; }

        ///<summary>
        /// 实际使用积分
        ///</summary>
        [ProtoMember(13)]
        public int PointRealUse { get; set; }

        ///<summary>
        /// 奖励积分
        ///</summary>
        [ProtoMember(14)]
        public int PointReward { get; set; }

        ///<summary>
        /// 优惠券
        ///</summary>
        [ProtoMember(15)]
        public string Coupon { get; set; }

        ///<summary>
        /// 优惠券抵消金额
        ///</summary>
        [ProtoMember(16)]
        public decimal CouponUse { get; set; }

        ///<summary>
        /// 礼品卡
        ///</summary>
        [ProtoMember(17)]
        public string GiftCard { get; set; }

        ///<summary>
        /// 礼品卡抵消金额
        ///</summary>
        [ProtoMember(18)]
        public decimal GiftCardUse { get; set; }

        ///<summary>
        /// 优惠详情json格式
        ///</summary>
        [JsonIgnore]
        [ProtoMember(19)]
        public string PromotionData { get; set; }

        ///<summary>
        /// 重量
        ///</summary>
        [ProtoMember(20)]
        public decimal Weight { get; set; }

        ///<summary>
        /// 电子凭证
        ///</summary>
        [ProtoMember(21)]
        public string ETicket { get; set; }

        ///<summary>
        /// 是否为虚拟物品 无需发货订单
        ///</summary>
        [ProtoMember(22)]
        public bool IsVirtual { get; set; }

        ///<summary>
        /// 是否为保税仓发货
        ///</summary>
        [ProtoMember(23)]
        public bool IsBonded { get; set; }

        ///<summary>
        /// 是否为海外直邮
        ///</summary>
        [ProtoMember(24)]
        public bool IsOversea { get; set; }

        ///<summary>
        /// 支付方式
        ///</summary>
        [ProtoMember(25)]
        public PaymentType PaymentType { get; set; }

        ///<summary>
        /// 支付方式信息
        ///</summary>
        [JsonIgnore]
        [ProtoMember(26)]
        public string PaymentData { get; set; }

        ///<summary>
        /// 配送方式
        ///</summary>
        [ProtoMember(27)]
        public ShipmentType ShipmentType { get; set; }

        ///<summary>
        /// 配送方式信息
        ///</summary>
        [JsonIgnore]
        [ProtoMember(28)]
        public string ShipmentData { get; set; }

        ///<summary>
        /// 超时时间
        ///</summary>
        [ProtoMember(29)]
        public DateTime ExpiredOn { get; set; }

        ///<summary>
        /// 发货日期
        ///</summary>
        [ProtoMember(30)]
        public DateTime? ConsignedOn { get; set; }

        ///<summary>
        /// 结束日期 交易成功时间(更新交易状态为成功的同时更新)/确认收货时间或者交易关闭时间
        ///</summary>
        [ProtoMember(31)]
        public DateTime? FinishedOn { get; set; }

        ///<summary>
        /// 0为不需要 1纸质发票 2为电子发票
        ///</summary>
        [ProtoMember(32)]
        public byte InvoiceType { get; set; }

        ///<summary>
        /// 发票抬头 个人或单位名称
        ///</summary>
        [ProtoMember(33)]
        public string InvoiceTitle { get; set; }

        ///<summary>
        /// 发票内容
        ///</summary>
        [ProtoMember(34)]
        public string InvoiceContent { get; set; }

        ///<summary>
        /// 发票状态 0未开 1已开纸质发票/已下载电子发票
        ///</summary>
        [ProtoMember(35)]
        public byte InvoiceStatus { get; set; }

        ///<summary>
        /// 物流公司运单号
        ///</summary>
        [ProtoMember(36)]
        public string LogisticsNo { get; set; }

        ///<summary>
        /// 0默认 1分拆成多个包裹发货 2多个订单合成一个包裹发货
        ///</summary>
        [ProtoMember(37)]
        public byte LogisticsType { get; set; }

        ///<summary>
        /// 物流公司编号
        ///</summary>
        [ProtoMember(38)]
        public string LogisticsCode { get; set; }

        ///<summary>
        /// 物流公司名称
        ///</summary>
        [ProtoMember(39)]
        public string LogisticsName { get; set; }

        ///<summary>
        /// 收货人
        ///</summary>
        [ProtoMember(40)]
        public string Receiver { get; set; }

        ///<summary>
        /// 固定电话
        ///</summary>
        [ProtoMember(41)]
        public string ReceiverPhone { get; set; }

        ///<summary>
        /// 移动电话
        ///</summary>
        [ProtoMember(42)]
        public string ReceiverMobile { get; set; }

        ///<summary>
        /// 行政区号
        ///</summary>
        [ProtoMember(43)]
        public string ReceiverRegionId { get; set; }

        ///<summary>
        /// 省
        ///</summary>
        [ProtoMember(44)]
        public string ReceiverProvince { get; set; }

        ///<summary>
        /// 市
        ///</summary>
        [ProtoMember(45)]
        public string ReceiverCity { get; set; }

        ///<summary>
        /// 区
        ///</summary>
        [ProtoMember(46)]
        public string ReceiverDistrict { get; set; }

        ///<summary>
        /// 街道门牌号
        ///</summary>
        [ProtoMember(47)]
        public string ReceiverStreet { get; set; }

        ///<summary>
        /// 邮编
        ///</summary>
        [ProtoMember(48)]
        public string ReceiverPostalCode { get; set; }

        ///<summary>
        /// 买家Id
        ///</summary>
        [ProtoMember(49)]
        public int BuyerId { get; set; }

        ///<summary>
        /// 买家用户名
        ///</summary>
        [ProtoMember(50)]
        public string BuyerName { get; set; }

        ///<summary>
        /// 买家备注旗帜 只有买家才能查看该字段
        ///</summary>
        [ProtoMember(51)]
        public byte BuyerFlag { get; set; }

        ///<summary>
        /// 买家备注
        ///</summary>
        [ProtoMember(52)]
        public string BuyerMemo { get; set; }

        ///<summary>
        /// 买家是否已评价 可选值:true(已评价),false(未评价) 如买家只评价未打分，此字段仍返回false
        ///</summary>
        [ProtoMember(53)]
        public bool BuyerRated { get; set; }

        ///<summary>
        /// 买家留言
        ///</summary>
        [ProtoMember(54)]
        public string BuyerMessage { get; set; }

        ///<summary>
        /// 卖家Id
        ///</summary>
        [ProtoMember(55)]
        public int SellerId { get; set; }

        ///<summary>
        /// 卖家用户名
        ///</summary>
        [ProtoMember(56)]
        public string SellerName { get; set; }

        ///<summary>
        /// 卖家备注Flag
        ///</summary>
        [ProtoMember(57)]
        public byte SellerFlag { get; set; }

        ///<summary>
        /// 卖家备注
        ///</summary>
        [ProtoMember(58)]
        public string SellerMemo { get; set; }

        ///<summary>
        /// 是否有退/换货
        ///</summary>
        [ProtoMember(59)]
        public bool HasReturn { get; set; }

        ///<summary>
        /// 是否有退款
        ///</summary>
        [ProtoMember(60)]
        public bool HasRefund { get; set; }

        ///<summary>
        /// 订单取消类型 0为none 1为系统取消 2为买家取消 3为卖家取消
        ///</summary>
        [ProtoMember(61)]
        public byte CancelId { get; set; }

        ///<summary>
        /// 订单取消原因
        ///</summary>
        [ProtoMember(62)]
        public string CancelReason { get; set; }

        ///<summary>
        /// 关联Id
        ///</summary>
        [ProtoMember(63)]
        public string RelatedId { get; set; }

        ///<summary>
        /// 媒体Id
        ///</summary>
        [ProtoMember(64)]
        public int MediaId { get; set; }

        ///<summary>
        /// 媒体跟踪码
        ///</summary>
        [ProtoMember(65)]
        public string TraceCode { get; set; }

        ///<summary>
        /// ip
        ///</summary>
        [ProtoMember(66)]
        public string IpAddress { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        [ProtoMember(67)]
        public string Extra { get; set; }

        ///<summary>
        /// 订单状态
        ///</summary>
        [ProtoMember(68)]
        public OrderStatus Status { get; set; }

        ///<summary>
        /// 订单来源 pc wap app 等
        ///</summary>
        [ProtoMember(69)]
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(70)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(71)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(72)]
        public DateTime ModifiedOn { get; set; }



        #region Readonly
        /// <summary>
        /// 未支付金额（待支付）
        /// </summary>
        public decimal Unpaid => Total - Paid;

        /// <summary>
        /// 取消文本
        /// </summary>
        public string CancelText
        {
            get
            {
                var s = string.Empty;

                switch (CancelId)
                {
                    case 0:
                        s = "系统"; break;
                    case 1:
                        s = "买家"; break;
                    case 2:
                        s = "卖家"; break;

                }

                return s;
            }
        }

        /// <summary>
        /// 订单状态文本
        /// </summary>
        public string StatusText
        {
            get
            {
                if (StatusDic.ContainsKey(Status))
                {
                    return StatusDic[Status];
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// 详细地址
        /// </summary>
        /// <returns></returns>
        public string Address
        {
            get
            {
                return ReceiverProvince + " " + ReceiverCity + " " + ReceiverDistrict;
            }
        }

        #endregion

        /// <summary>
        /// 订单状态字典
        /// </summary>
        public static IDictionary<OrderStatus, string> StatusDic
        {
            get
            {
                var dic = new Dictionary<OrderStatus, string>();
                dic.Add(OrderStatus.None, "");
                dic.Add(OrderStatus.Pending, "待处理");
                dic.Add(OrderStatus.WaitBuyerPay, "待支付");
                dic.Add(OrderStatus.WaitSellerSend, "待发货");

                dic.Add(OrderStatus.SellerSendPart, "已部分发货");
                dic.Add(OrderStatus.WaitBuyerConfirm, "已发货");

                dic.Add(OrderStatus.Canceled, "已取消");
                dic.Add(OrderStatus.Closed, "已关闭");
                dic.Add(OrderStatus.Finished, "已完成");
                dic.Add(OrderStatus.Deleted, "已删除");
                return dic;
            }
        }

        public static IDictionary<byte, string> FlagColorDic
        {
            get
            {
                var dic = new Dictionary<byte, string>();
                dic.Add(0, "");
                dic.Add(1, "c-red");
                dic.Add(2, "c-orange");
                dic.Add(3, "c-green");
                dic.Add(4, "c-blue");
                dic.Add(5, "c-purple");
                return dic;
            }
        }

        /*
        public static IDictionary<OrderCategory, string> CategoryDic
        {
            get
            {
                var dic = new Dictionary<OrderCategory, string>();
                dic.Add(OrderCategory.Normal, "网购订单");
                dic.Add(OrderCategory.Auction, "竞购订单");
                dic.Add(OrderCategory.AuctionVariant, "竞购补差订单");
                dic.Add(OrderCategory.IntegralExchange, "积分兑换订单");
                return dic;
            }
        }
        */
        #endregion


        #region Navigation Properties

        /// <summary>
        /// 订单扩展信息
        /// </summary>
        //public virtual OrderExtra OrderExtra { get; set; }

        /// <summary>
        /// 子订单
        /// </summary>
        [ProtoMember(100)]

        public virtual IList<OrderItem> Items { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        [ProtoMember(101)]

        public virtual IList<OrderNote> Notes { get; set; }

        /// <summary>
        /// 订单使用的折扣
        /// </summary>
        //public virtual IList<DiscountUsageHistory> DiscountUsageHistory { get; set; }

        ///// <summary>
        ///// 订单使用的礼品卡
        ///// </summary>
        //public virtual IList<GiftCardUsageHistory> GiftCardUsageHistory { get; set; }

        #endregion


        public Status Validate()
        {
            return new Status();
        }

        /// <summary>
        /// 计算退还积分
        /// </summary>
        /// <param name="id">OrderItem.Id</param>
        /// <returns></returns>
        public int GetRefundPoint(string id)
        {
            var items = Items;
            if (items == null) return 0;
            if (items.Count == 1)
            {
                return PointUse;
            }
            var item = items.FirstOrDefault(o => o.Id == id);
            return item != null ? Convert.ToInt32((PointUse * item.Total / items.Sum(o => o.Total))) : 0;
        }

        /// <summary>
        /// 计算退款金额
        /// </summary>
        /// <param name="id">OrderItem.Id</param>
        /// <returns></returns>
        public decimal GetRefundAmount(string id)
        {
            var items = Items;
            if (items == null) return 0;

            if (items.Count == 1)
            {
                var item = items.FirstOrDefault(o => o.Id == id);
                if (item != null)
                {
                    return item.Total - (Discount * item.Total / item.Total) + Freight;
                }
                return 0;
            }
            else
            {
                var item = items.FirstOrDefault(o => o.Id == id);
                if (item == null) return 0;

                var isLast = items.Count(o => o.Id != id && !string.IsNullOrEmpty(o.RefundId) && o.RefundStatus == 1) == items.Count - 1;
                if (isLast)
                {
                    return item.Total - (Discount * item.Total / items.Sum(o => o.Total)) + Freight;
                }
                return item.Total - (Discount * item.Total / items.Sum(o => o.Total));
            }
        }

        #region
        /// <summary>
        /// The inclock.
        /// </summary>
        private static readonly object Inclock = new object();

        /// <summary>
        /// The inc.
        /// </summary>
        private static int inc;

        /// <summary>
        /// Generate an increment.
        /// </summary>
        /// <returns>
        /// The increment.
        /// </returns>
        private static int GenerateInc()
        {
            lock (Inclock)
            {
                if (inc > 99)
                {
                    inc = 0;
                }
                else
                {
                    inc++;
                }
                return inc;
            }
        }
        #endregion

        /// <summary>
        /// 生成一个新的订单Id
        /// </summary>
        /// <param name="buyerId">买家Id</param>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public static string NewId(int buyerId, int sellerId)
        {
            var now = DateTime.Now;
            var i = GenerateInc();
            return now.ToString("yyMMddHHmmss") + (buyerId % 64).ToString("00") + (sellerId % 64).ToString("00") + i.ToString("00");
        }
    }

    /// <summary>
    /// 订单Mini
    /// </summary>
    public class OrderMini : IEntity<string>
    {
        ///<summary>
        /// 订单Id
        ///</summary>
        public string Id { get; set; }

        ///<summary>
        /// 订单类型:0网购订单 1竞购订单 2竞购补差订单 3积分兑换订单
        ///</summary>
        public OrderType Type { get; set; }

        ///<summary>
        /// 商品总数量
        ///</summary>
        public int Quantity { get; set; }

        ///<summary>
        /// 订单小计(商品总金额)
        ///</summary>
        public decimal SubTotal { get; set; }

        ///<summary>
        /// 运费
        ///</summary>
        public decimal Freight { get; set; }

        ///<summary>
        /// 关税税费
        ///</summary>
        public decimal Tax { get; set; }

        ///<summary>
        /// 订单优惠总金额(使用积分抵扣 优惠券 店铺优惠等)
        ///</summary>
        public decimal Discount { get; set; }

        ///<summary>
        /// 订单总金额(需要支付的总金额[SubTotal+Postage-Discount])
        ///</summary>
        public decimal Total { get; set; }

        ///<summary>
        /// 已支付金额(一个订单可能分多次支付)
        ///</summary>
        public decimal Paid { get; set; }

        ///<summary>
        /// 支付流水号
        ///</summary>
        public string PayId { get; set; }

        ///<summary>
        /// 支付日期(最后一次支付完成整个订单的时间)
        ///</summary>
        public DateTime? PaidOn { get; set; }

        ///<summary>
        /// 使用积分
        ///</summary>
        public int PointUse { get; set; }

        ///<summary>
        /// 实际使用积分
        ///</summary>
        public int PointRealUse { get; set; }

        ///<summary>
        /// 奖励积分
        ///</summary>
        public int PointReward { get; set; }

        ///<summary>
        /// 优惠券
        ///</summary>
        public string Coupon { get; set; }

        ///<summary>
        /// 优惠券抵消金额
        ///</summary>
        public decimal CouponUse { get; set; }

        ///<summary>
        /// 礼品卡
        ///</summary>
        public string GiftCard { get; set; }

        ///<summary>
        /// 礼品卡抵消金额
        ///</summary>
        public decimal GiftCardUse { get; set; }

        ///<summary>
        /// 重量
        ///</summary>
        public decimal Weight { get; set; }

        ///<summary>
        /// 电子凭证
        ///</summary>
        public string ETicket { get; set; }

        ///<summary>
        /// 是否为虚拟物品 无需发货订单
        ///</summary>
        public bool IsVirtual { get; set; }

        ///<summary>
        /// 是否为保税仓发货
        ///</summary>
        public bool IsBonded { get; set; }

        ///<summary>
        /// 是否为海外直邮
        ///</summary>
        public bool IsOversea { get; set; }

        ///<summary>
        /// 支付方式
        ///</summary>
        public PaymentType PaymentType { get; set; }


        ///<summary>
        /// 配送方式
        ///</summary>
        public ShipmentType ShipmentType { get; set; }

        ///<summary>
        /// 超时时间
        ///</summary>
        public DateTime ExpiredOn { get; set; }

        ///<summary>
        /// 买家Id
        ///</summary>
        public int BuyerId { get; set; }

        ///<summary>
        /// 买家用户名
        ///</summary>
        public string BuyerName { get; set; }

        ///<summary>
        /// 卖家Id
        ///</summary>
        public int SellerId { get; set; }

        ///<summary>
        /// 卖家用户名
        ///</summary>
        public string SellerName { get; set; }

        ///<summary>
        /// 媒体Id
        ///</summary>
        public int MediaId { get; set; }

        ///<summary>
        /// 媒体跟踪码
        ///</summary>
        public string TraceCode { get; set; }

        ///<summary>
        /// 订单状态
        ///</summary>
        public OrderStatus Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 未支付金额（待支付）
        /// </summary>
        public decimal Unpaid => Total - Paid;

        public Payment ToPayment(int platformId, string ipAddress)
        {
            var now = DateTime.Now;
            var payment = new Payment();

            payment.Id = Payment.NewId();

            payment.No = string.Empty;
            payment.Type = 0;
            payment.UserId = BuyerId;
            payment.Method = 0;
            payment.Amount = Unpaid;//待支付金额=总金额-已支付金额
            payment.Subject = "test";
            payment.Message = "";
            payment.RawData = "";
            payment.BankId = 0;
            payment.BankCode = string.Empty;
            payment.BankName = string.Empty;
            payment.BankCard = 0;
            payment.PlatformId = platformId;
            payment.CardNo = string.Empty;
            payment.CardPwd = string.Empty;
            payment.CardAmount = 0;
            payment.RelatedId = Id;
            payment.IpAddress = ipAddress;
            payment.Extra = string.Empty;
            payment.Status = false;
            payment.CreatedBy = "sys";
            payment.CreatedOn = now;
            payment.ModifiedBy = string.Empty;
            payment.ModifiedOn = now;
            return payment;
        }

    }

}