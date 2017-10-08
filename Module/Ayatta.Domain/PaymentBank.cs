using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// 支付平台银行
    ///</summary>
    [ProtoContract]
    public class PaymentBank : IEntity<int>
    {
        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 银行Id
        ///</summary>
        [ProtoMember(2)]
        public int BankId { get; set; }

        ///<summary>
        /// 支付平台Id
        ///</summary>
        [ProtoMember(3)]
        public int PlatformId { get; set; }

        ///<summary>
        /// 银行在支付平台的Code
        ///</summary>
        [ProtoMember(4)]
        public string Code { get; set; }

        ///<summary>
        /// 描述
        ///</summary>
        [ProtoMember(5)]
        public string Description { get; set; }

        ///<summary>
        /// 排序优先级 从小到大
        ///</summary>
        [ProtoMember(6)]
        public int Priority { get; set; }

        ///<summary>
        /// 徽章 标记 用于活动时提醒
        ///</summary>
        [ProtoMember(7)]
        public string Badge { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        [ProtoMember(8)]
        public string Extra { get; set; }

        ///<summary>
        /// 状态 1可用 0不可用
        ///</summary>
        [ProtoMember(9)]
        public bool Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(10)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(11)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(12)]
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        [ProtoMember(100)]
        public virtual Bank Bank { get; set; }

        /// <summary>
        /// 支付平台
        /// </summary>
        [ProtoMember(101)]
        public virtual PaymentPlatform Platform { get; set; }
    }
}