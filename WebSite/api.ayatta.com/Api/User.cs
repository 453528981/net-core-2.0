
using Ayatta.Domain;

namespace Ayatta.Web.Api
{
    #region 用户
    /// <summary>
    /// 用户信息获取 响应
    /// </summary>
    public sealed class UserGetResponse : Response
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public User Data { get; set; }

    }

    /// <summary>
    /// 用户信息获取 请求
    /// </summary>
    public sealed class UserGetRequest : Request<UserGetResponse>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Profile { get; set; }
    }
    #endregion

    #region 用户是否已存在
    /// <summary>
    /// 用户是否已存在 响应
    /// </summary>
    public sealed class UserExistResponse : Response
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public bool Data { get; set; }

    }

    /// <summary>
    /// 用户是否已存在 请求
    /// </summary>
    public sealed class UserExistRequest : Request<UserExistResponse>
    {
        /// <summary>
        /// 用户名/手机/邮箱
        /// </summary>
        public string Uid { get; set; }
    }
    #endregion

    #region 用户注册
    /// <summary>
    /// 用户注册 响应
    /// </summary>
    public sealed class UserSignUpResponse : Response
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public User Data { get; set; }

    }

    /// <summary>
    /// 用户注册 请求
    /// </summary>
    public sealed class UserSignUpRequest : Request<UserSignUpResponse>
    {
        /// <summary>
        /// 用户名/手机号/邮箱
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 验证码唯一识别码
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 定手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string Captcha { get; set; }
    }
    #endregion
}