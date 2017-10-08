using System;
using ProtoBuf;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    ///<summary>
    /// PaymentPlatform
    ///</summary>
    [ProtoContract]
    public class PaymentPlatform : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 支付平台名称
        ///</summary>
        [ProtoMember(2)]
        public string Name { get; set; }

        ///<summary>
        /// 支付平台图标
        ///</summary>
        [ProtoMember(3)]
        public string IconSrc { get; set; }

        ///<summary>
        /// 支付平台分配的商户编号
        ///</summary>
        [ProtoMember(4)]
        public string MerchantId { get; set; }

        ///<summary>
        /// 私钥
        ///</summary>
        [ProtoMember(5)]
        public string PrivateKey { get; set; }

        ///<summary>
        /// 支付平台分配的共钥
        ///</summary>
        [ProtoMember(6)]
        public string PublicKey { get; set; }

        ///<summary>
        /// 支付平台网关
        ///</summary>
        [ProtoMember(7)]
        public string GatewayUrl { get; set; }

        ///<summary>
        /// 支付回调URL
        ///</summary>
        [ProtoMember(8)]
        public string CallbackUrl { get; set; }

        ///<summary>
        /// 支付通知URL
        ///</summary>
        [ProtoMember(9)]
        public string NotifyUrl { get; set; }

        ///<summary>
        /// 查询URL
        ///</summary>
        [ProtoMember(10)]
        public string QueryUrl { get; set; }

        ///<summary>
        /// 退款URL
        ///</summary>
        [ProtoMember(11)]
        public string RefundUrl { get; set; }

        ///<summary>
        /// 描述
        ///</summary>
        [ProtoMember(12)]
        public string Description { get; set; }

        ///<summary>
        /// 排序优先级 从小到大
        ///</summary>
        [ProtoMember(13)]
        public int Priority { get; set; }

        ///<summary>
        /// 徽章 标记 用于活动时提醒
        ///</summary>
        [ProtoMember(14)]
        public string Badge { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        [ProtoMember(15)]
        public string Extra { get; set; }

        ///<summary>
        /// 状态 1可用 0不可用
        ///</summary>
        [ProtoMember(16)]
        public bool Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(17)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(18)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(19)]
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// 所支持网上银行
        /// </summary>
        [ProtoMember(100)]
        public virtual IEnumerable<PaymentBank> Banks { get; set; }
    }
}