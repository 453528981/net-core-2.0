using System;
using ProtoBuf;

namespace Ayatta.Domain
{

    ///<summary>
    /// UserInvoice
    /// created on 2016-07-02 15:04:51
    ///</summary>
    [ProtoContract]
    public class UserInvoice : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// UserId
        ///</summary>
        [ProtoMember(2)]
        public int UserId { get; set; }

        ///<summary>
        /// 0为普��?1为增值
        ///</summary>
        [ProtoMember(3)]
        public byte GroupId { get; set; }

        ///<summary>
        /// 发票抬头 个人或单位名称
        ///</summary>
        [ProtoMember(4)]
        public string Title { get; set; }

        ///<summary>
        /// 发票内容
        ///</summary>
        [ProtoMember(5)]
        public string Content { get; set; }

        ///<summary>
        /// 是否为默认
        ///</summary>
        [ProtoMember(6)]
        public bool IsDefault { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(7)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(8)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(9)]
        public DateTime ModifiedOn { get; set; }
    }

}