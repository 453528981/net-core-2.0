using System;
using ProtoBuf;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace Ayatta.Domain
{
    ///<summary>
    /// User
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class User : IEntity<int>
    {
        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// Guid
        ///</summary>
        public string Guid { get; set; }

        ///<summary>
        /// 登录帐号 用户名/绑定的邮箱/绑定的手机号
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// 绑定的邮箱
        ///</summary>
        public string Email { get; set; }

        ///<summary>
        /// 绑定的手机号
        ///</summary>
        public string Mobile { get; set; }

        ///<summary>
        /// 用户昵称
        ///</summary>
        public string Nickname { get; set; }

        ///<summary>
        /// 用户登录密码
        ///</summary>
        [JsonIgnore, ProtoIgnore]
        public string Password { get; set; }

        ///<summary>
        /// 用户角色 买家 卖家 平台管理员
        ///</summary>
        public UserRole Role { get; set; }

        ///<summary>
        /// 用户级别
        ///</summary>
        public UserGrade Grade { get; set; }

        /*
        /// <summary>
        /// 角色Id 用户权限通过该值获取
        /// </summary>
        public int RoleId { get; set; }
        */

        /// <summary>
        /// 父Id
        /// </summary>
        public int ParentId { get; set; }

        ///<summary>
        /// 用户限制
        ///</summary>
        public UserLimitation Limitation { get; set; }

        ///<summary>
        /// 商家许可
        ///</summary>
        public UserPermission Permission { get; set; }

        ///<summary>
        /// Avatar
        ///</summary>
        public string Avatar { get; set; }

        ///<summary>
        /// 0正常 1未通过手机 邮箱验证 2被系统隔离 无法下单 3被系统禁用 帐号异常或违规  255被系统删除 无法进行任何操作
        ///</summary>
        public UserStatus Status { get; set; }

        ///<summary>
        /// 通过真实身份验证时间
        ///</summary>
        public DateTime? AuthedOn { get; set; }

        ///<summary>
        /// 通过qq sina 等注册
        ///</summary>
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Profile
        /// </summary>
        /// <returns></returns>
        public virtual UserProfile Profile { get; set; }

        /// <summary>
        /// 是否为子帐号
        /// </summary>
        public bool IsSubAccount
        {
            get
            {
                return ParentId > 0;
            }
        }


        #region

        /// <summary>
        /// 性别字典
        /// </summary>
        public static IDictionary<Gender, string> UserGenderDic
        {
            get
            {
                var dic = new Dictionary<Gender, string>();
                dic.Add(Gender.Secrect, "保密");
                dic.Add(Gender.Male, "男");
                dic.Add(Gender.Female, "女");
                return dic;
            }
        }

        /// <summary>
        /// 婚姻状态字典
        /// </summary>
        public static IDictionary<Marital, string> MaritalStatusDic
        {
            get
            {
                var dic = new Dictionary<Marital, string>();
                dic.Add(Marital.Secrect, "保密");
                dic.Add(Marital.Single, "未婚");
                dic.Add(Marital.Married, "已婚");
                return dic;
            }
        }
        #endregion


    }
}