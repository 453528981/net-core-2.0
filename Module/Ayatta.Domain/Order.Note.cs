using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// OrderNote
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class OrderNote : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 类型
        ///</summary>
        public int Type { get; set; }

        ///<summary>
        /// UserId
        ///</summary>
        public int UserId { get; set; }

        ///<summary>
        /// 订单Id
        ///</summary>
        public string OrderId { get; set; }

        ///<summary>
        /// 主题
        ///</summary>
        public string Subject { get; set; }

        ///<summary>
        /// 消息
        ///</summary>
        public string Message { get; set; }

        ///<summary>
        /// 扩展
        ///</summary>
        public string Extra { get; set; }

        ///<summary>
        /// 创建者
        ///</summary>
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        public DateTime CreatedOn { get; set; }
    }
}