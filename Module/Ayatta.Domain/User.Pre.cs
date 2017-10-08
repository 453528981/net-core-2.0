using System;
using ProtoBuf;
namespace Ayatta.Domain
{
    ///<summary>
    /// UserPre
    /// 预注册用户信息 通过邮箱注册的用户 首先会写入该表 通过邮箱里的链接认证通过后才会写入User表
    /// created on 2016-07-02 15:50:43
    ///</summary>
    [ProtoContract]
    public class UserPre : IEntity<string>
    {

        ///<summary>
        /// Id 验证成功后对应User表中的Guid
        ///</summary>
        [ProtoMember(1)]
        public string Id {get;set;}

        ///<summary>
        /// 待验证的Email
        ///</summary>
        [ProtoMember(2)]
        public string Name {get;set;}

        ///<summary>
        /// 密码
        ///</summary>
        [ProtoMember(3)]
        public string Password {get;set;}

        ///<summary>
        /// Browser
        ///</summary>
        [ProtoMember(4)]
        public string Browser {get;set;}

        ///<summary>
        /// UserAgent
        ///</summary>
        [ProtoMember(5)]
        public string UserAgent {get;set;}

        ///<summary>
        /// IpAddress
        ///</summary>
        [ProtoMember(6)]
        public string IpAddress {get;set;}

        ///<summary>
        /// UrlReferrer
        ///</summary>
        [ProtoMember(7)]
        public string UrlReferrer {get;set;}

        ///<summary>
        /// 媒体Id
        ///</summary>
        [ProtoMember(8)]
        public int MediaId {get;set;}

        ///<summary>
        /// 媒体跟踪码
        ///</summary>
        [ProtoMember(9)]
        public string TraceCode {get;set;}

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(10)]
        public DateTime CreatedOn {get;set;}
    }
}