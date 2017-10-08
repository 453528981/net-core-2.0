using System;
using ProtoBuf;
using System.Collections.Generic;


namespace Ayatta.Domain
{
    /// <summary>
    /// UserProfile
    /// </summary>
    public class UserProfile : IEntity<int>
    {
        ///<summary>
        /// UserId
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 身份证号
        ///</summary>
        [ProtoMember(2)]
        public string Code { get; set; }

        ///<summary>
        /// 真实姓名
        ///</summary>
        [ProtoMember(3)]
        public string Name { get; set; }

        ///<summary>
        /// 0为保密 1为男 2为女
        ///</summary>
        [ProtoMember(4)]
        public Gender Gender { get; set; }

        ///<summary>
        /// 0为保密 1为单身 2为已婚
        ///</summary>
        [ProtoMember(5)]
        public Marital Marital { get; set; }

        ///<summary>
        /// 出生日期
        ///</summary>
        [ProtoMember(6)]
        public DateTime? Birthday { get; set; }

        ///<summary>
        /// 固定电话
        ///</summary>
        [ProtoMember(7)]
        public string Phone { get; set; }

        ///<summary>
        /// 移动电话
        ///</summary>
        [ProtoMember(8)]
        public string Mobile { get; set; }

        ///<summary>
        /// 所属省市区Id
        ///</summary>
        [ProtoMember(9)]
        public string RegionId { get; set; }

        ///<summary>
        /// 街道门牌号
        ///</summary>
        [ProtoMember(10)]
        public string Street { get; set; }

        ///<summary>
        /// 注册Ip
        ///</summary>
        [ProtoMember(11)]
        public string SignUpIp { get; set; }

        ///<summary>
        /// 0为通过用户名注册，1为通过邮箱注册，2为通过手机号码注册，3为通过手机短信注册
        ///</summary>
        [ProtoMember(12)]
        public byte SignUpBy { get; set; }

        ///<summary>
        /// 注册跟踪码
        ///</summary>
        [ProtoMember(13)]
        public string TraceCode { get; set; }

        ///<summary>
        /// 最后一次登录Ip
        ///</summary>
        [ProtoMember(14)]
        public string LastSignInIp { get; set; }

        ///<summary>
        /// 最后一次登录时间
        ///</summary>
        [ProtoMember(15)]
        public DateTime LastSignInOn { get; set; }


    }

}