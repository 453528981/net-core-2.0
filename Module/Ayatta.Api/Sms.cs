using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayatta.Api
{
    #region 手机短信发送
    /// <summary>
    /// 手机短信发送 响应
    /// </summary>
    public sealed class SmsSendResponse : Response
    {
        /// <summary>
        /// 短信唯一识别码（验证时需要）
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string Data { get; set; }

    }

    /// <summary>
    /// 手机短信发送 请求
    /// </summary>
    public sealed class SmsSendRequest : Request<SmsSendResponse>
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte GroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string Extra { get; set; }
    }
    #endregion
}
