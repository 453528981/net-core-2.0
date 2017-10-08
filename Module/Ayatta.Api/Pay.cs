using System;
using System.Collections.Generic;
using System.Text;

namespace Ayatta.Api
{
    #region 用户注册
    /// <summary>
    /// 支付宝支付 响应
    /// </summary>
    public sealed class AlipayResponse : Response
    {
        /// <summary>
        /// 支付宝支付信息
        /// </summary>
        public string Data { get; set; }

    }

    /// <summary>
    /// 支付宝支付 请求
    /// </summary>
    public sealed class AlipayRequest : Request<AlipayResponse>
    {
        /// <summary>
        /// 支付单Id
        /// </summary>
        public string PayId { get; set; }
    }
    #endregion
}
