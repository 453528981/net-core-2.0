using System;
using Ayatta.Domain;
using Microsoft.AspNetCore.Http;

namespace Ayatta.OAuth
{
    public interface IAuthProvider
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// State
        /// </summary>
        string State { get; }

        /// <summary>
        /// 用户在第三方平台登录成功并授权后触发事件
        /// </summary>
        Func<UserOAuth, int> Authorized { get; set; }

        /// <summary>
        /// 生成登录url
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        string GetLoginUri(string state = null);

        /// <summary>
        /// 授权回调
        /// </summary>
        /// <param name="param">授权参数</param>
        /// <returns></returns>
        Result<UserOAuth> Callback(IQueryCollection param);
    }
}
