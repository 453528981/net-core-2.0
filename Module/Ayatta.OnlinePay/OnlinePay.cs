using System;
using System.Text;
using Ayatta.Domain;
using System.Net.Http;
using System.Security.Cryptography;

namespace Ayatta.OnlinePay
{
    public abstract class OnlinePay : IOnlinePay
    {
        protected const int Timeout = 5000;
        protected readonly HttpClient Client;

        //protected string BaseUrl { get; set; }
        //protected string DefaultKey { get; set; }

        /// <summary>
        /// 支付平台名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 接收到支付平台通知并验证通过后触发 用于处理系统内支付逻辑
        /// </summary>
        public Func<Notification, bool> Notified { get; set; }

        /// <summary>
        /// 输出Log
        /// </summary>
        public Action<string, string> Traced { get; set; }

        /// <summary>
        /// 支付平台信息
        /// </summary>
        protected PaymentPlatform Platform { get; private set; }

        /// <summary>
        /// 在线支付
        /// </summary>
        /// <param name="platform">支付平台配置信息</param>
        protected OnlinePay(PaymentPlatform platform)
        {
            Platform = platform;
            Name = platform.Name;
            Client = new HttpClient { BaseAddress = new Uri(platform.GatewayUrl) };

            if (Platform.NotifyUrl.EndsWith("notify/"))
            {
                Platform.NotifyUrl = Platform.NotifyUrl + Platform.Id;
            }
            if (Platform.CallbackUrl.EndsWith("callback/"))
            {
                Platform.CallbackUrl = Platform.CallbackUrl + Platform.Id;
            }
        }

        /// <summary>
        /// 接收到第三方平台通知并验证通过后触发
        /// </summary>
        /// <param name="e"></param>
        protected virtual bool OnNotified(Notification e)
        {
            var handler = Notified;
            return handler != null && handler(e);
        }

        /// <summary>
        /// 输出Log
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="message">消息</param>
        protected virtual void OnTraced(string subject, string message)
        {
            var handler = Traced;
            handler?.Invoke(subject, message);
        }

        /// <summary>
        /// 生成支付所需信息 url json等
        /// </summary>
        /// <param name="payment">付款信息</param>
        /// <param name="exts">其他参数信息</param>
        /// <returns></returns>
        public abstract string Pay(Payment payment, params string[] exts);

        /// <summary>
        /// 处理支付平台通知
        /// </summary>
        /// <param name="param">支付平台通知参数</param>
        /// <returns></returns>
        public abstract Result<Notification> HandleNotify(PaymentParam param);

        /// <summary>
        /// 生成Md5摘要
        /// </summary>
        /// <param name="content"></param>
        /// <param name="upper">是否转为大写</param>
        /// <returns></returns>
        protected static string Hash(string content, bool upper = false)
        {
            var input = Encoding.UTF8.GetBytes(content);

            var output = MD5.Create().ComputeHash(input);
            var sb = new StringBuilder();
            foreach (var b in output)
            {
                sb.Append(upper ? b.ToString("X2") : b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}