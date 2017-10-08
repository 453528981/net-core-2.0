
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Ayatta.Sms
{
    /// <summary>
    /// 
    /// </summary>
    public class SmsOptions : IOptions<SmsOptions>
    {
        /// <summary>
        /// 短信网关BaseUrl
        /// </summary>
        public string SmsBaseUrl { get; set; } = "http://61.145.229.29:9003/";

        /// <summary>
        /// 黑名单 多个以(,)分隔
        /// </summary>
        public string Blacklist { get; set; }

        /// <summary>
        /// 是否存贮
        /// </summary>
        public bool EnabledStorage { get; set; } = false;

        /// <summary>
        /// 存贮Storage
        /// </summary>
        public ISmsStorage SmsStorage { get; set; }


        SmsOptions IOptions<SmsOptions>.Value => this;

        //public IList<IThrottle> Throttles { get; set; }
    }

    public interface IThrottle
    {
        /// <summary>
        /// 间隔 以分钟为单位
        /// </summary>
        int Span { get; set; }

        IDictionary<string, int> Topics { get; set; }
    }

    /// <summary>
    /// 每个ip在指定时间间隔内可发送最大数量
    /// </summary>
    public class IpThrottle : IThrottle
    {
        public int Span { get; set; } = 10;
        public IDictionary<string, int> Topics { get; set; }
    }

    public class DeviceThrottle : IThrottle
    {
        public int Span { get; set; } = 10;

        public IDictionary<string, int> Topics { get; set; }

    }
}