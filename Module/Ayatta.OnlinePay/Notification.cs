using System;

namespace Ayatta.OnlinePay
{
    /// <summary>
    /// 支付平台支付通知
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// 支付单号
        /// </summary>
        public string PayId { get; set; }

        /// <summary>
        /// 支付平台交易号
        /// </summary>
        public string PayNo { get; set; }

        /// <summary>
        /// 支付订单金额 保留两位小数(以元为单位)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PaidOn { get; set; }

        /// <summary>
        /// 第三方支付平台Id
        /// </summary>
        public int PlatformId { get; private set; }

        /// <summary>
        /// 状态 支付平台原样返回
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 支付平台发送过来的通知原数据(转成xml格式 )
        /// </summary>
        public string Input { get; private set; }

        /// <summary>
        /// 系统处理完支付平台发来的通知后需回复给支付平台的数据
        /// </summary>
        public string Output { get; private set; }

        /// <summary>
        /// 处理结果消息
        /// </summary>
        //public string Message { get; set; }

        /// <summary>
        /// 通知处理状态 系统成功处理后为true
        /// </summary>
        //public bool Status { get; set; }

        public Notification(int platformId) : this(platformId, string.Empty, string.Empty)
        {
            PlatformId = platformId;
        }

        public Notification(int platformId, string intput, string output)
        {
            PlatformId = platformId;
            Input = intput;
            Output = output;
        }

        public void SetOutput(string output)
        {
            Output = output;
        }

    }
}