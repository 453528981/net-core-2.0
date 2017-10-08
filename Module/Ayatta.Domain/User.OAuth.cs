using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// UserOAuth
    /// created on 2016-07-02 15:22:52
    ///</summary>
    [ProtoContract]
    public class UserOAuth : IEntity<int>
    {

        ///<summary>
        /// UserId
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// OpenId
        ///</summary>
        [ProtoMember(2)]
        public string OpenId { get; set; }

        ///<summary>
        /// Provider (qq sina等)
        ///</summary>
        [ProtoMember(3)]
        public string Provider { get; set; }

        ///<summary>
        /// OpenName
        ///</summary>
        [ProtoMember(4)]
        public string OpenName { get; set; }

        ///<summary>
        /// Scope
        ///</summary>
        [ProtoMember(5)]
        public string Scope { get; set; }

        ///<summary>
        /// AccessToken
        ///</summary>
        [ProtoMember(6)]
        public string AccessToken { get; set; }

        ///<summary>
        /// RefreshToken
        ///</summary>
        [ProtoMember(7)]
        public string RefreshToken { get; set; }

        ///<summary>
        /// AccessToken有效期
        ///</summary>
        [ProtoMember(8)]
        public DateTime ExpiredOn { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [ProtoMember(9)]
        public string Extra { get; set; }

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
    }

}