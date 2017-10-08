using System;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ayatta.Sms
{
    public class SmsService : ISmsService
    {
        private readonly ILogger logger;
        private readonly SmsOptions options;
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// 默认数据存贮
        /// </summary>
        /// <param name="optionsAccessor"></param>
        /// <param name="logger"></param>
        public SmsService(IOptions<SmsOptions> optionsAccessor, ILogger<SmsService> logger)
        {
            this.logger = logger;

            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            options = optionsAccessor.Value;


            client.BaseAddress = new Uri(options.SmsBaseUrl);
        }

        public async Task<SmsResut> SendMessage(string mobile, string message, string topic = "", int uid = 0)
        {
            if (!string.IsNullOrEmpty(options.Blacklist))
            {
                var list = options.Blacklist.Split(',');
                if (list.Contains(mobile))
                {
                    return new SmsResut { Guid = string.Empty, Status = false, Message = "该手机号已被列入黑名单。" };
                }
            }
            var msg = new SmsMessage();
            var guid = SmsMessage.NewId();
            msg.Id = guid;
            msg.Topic = topic;
            msg.Mobile = mobile;
            msg.Message = message;
            msg.Failure = string.Empty;
            msg.Status = Status.Pending;
            msg.CreatedOn = DateTime.Now;

            try
            {
                var val = await SendSms(mobile, message);

                if (string.IsNullOrEmpty(val)/* || val.StartsWith("-")*/)
                {
                    msg.Status = Status.Failed;
                    msg.Message = val;
                }
                else
                {
                    msg.Id = val;
                    msg.Status = Status.Successful;
                }

                if (logger.IsEnabled(LogLevel.Information))
                {
                    var info = string.Format("唯一识别码{0} 手机号{1} 主题{2} 内容{3}", msg.Id, mobile, topic, message);
                    logger.LogWarning("发送消息：" + info);
                }

                SaveSms(msg);

                return new SmsResut { Guid = msg.Id, Status = msg.Status == Status.Successful };
            }
            catch (Exception e)
            {
                logger.LogError("发送短信异常：" + e.Message);
                return new SmsResut { Guid = msg.Id, Status = false, Message = e.Message };
            }
        }
        private void SaveSms(SmsMessage msg)
        {
            if (options.EnabledStorage && options.SmsStorage != null)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var v = options.SmsStorage.Save(msg);
                        if (!v)
                        {
                            var s = string.Format("唯一识别码{0} 手机号{1} 主题{2} 内容{3}", msg.Id, msg.Mobile, msg.Topic, msg.Message);
                            logger.LogWarning("写入数据失败：" + s);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError("写入数据异常：" + e.Message);
                    }
                });
            }
        }
        private async Task<string> SendSms(string mobile, string message)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("iMobiCount", "1");
            dic.Add("pszSubPort", "*");
            dic.Add("userId", "J00516");
            dic.Add("password", "753357");
            dic.Add("pszMobis", mobile);
            dic.Add("pszMsg", message);

            var content = new FormUrlEncodedContent(dic);
            var url = "MWGate/wmgw.asmx/MongateCsSpSendSmsNew";

            var response = await client.PostAsync(url, content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    return XElement.Load(stream).Value;
                }
            }
            else
            {
                return response.ReasonPhrase;
            }
        }
    }
}
