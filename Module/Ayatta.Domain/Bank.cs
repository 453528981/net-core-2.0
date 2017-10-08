using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// Bank
    ///</summary>
    [ProtoContract]
    public class Bank : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 银行名称
        ///</summary>
        [ProtoMember(2)]
        public string Name { get; set; }

        ///<summary>
        /// 银行图标
        ///</summary>
        [ProtoMember(3)]
        public string IconSrc { get; set; }

        ///<summary>
        /// 描述
        ///</summary>
        [ProtoMember(4)]
        public string Description { get; set; }

        ///<summary>
        /// 排序优先级 从小到大
        ///</summary>
        [ProtoMember(5)]
        public int Priority { get; set; }

        ///<summary>
        /// 徽章 标记 用于活动时提醒
        ///</summary>
        [ProtoMember(6)]
        public string Badge { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        [ProtoMember(7)]
        public string Extra { get; set; }

        ///<summary>
        /// 状态 true可用 false不可用
        ///</summary>
        [ProtoMember(8)]
        public bool Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(9)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(10)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(11)]
        public DateTime ModifiedOn { get; set; }
    }
}