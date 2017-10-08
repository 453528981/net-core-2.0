using System;
using System.Linq;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ayatta.Weed
{
    public class WeedService : IWeedService
    {
        private readonly ILogger logger;
        private readonly WeedOptions options;
        private static readonly HttpClient client = new HttpClient();

        public Action<UploadResult> OnUpload { get; set; }

        /// <summary>
        /// 默认数据存贮
        /// </summary>
        /// <param name="optionsAccessor"></param>
        /// <param name="logger"></param>
        public WeedService(IOptions<WeedOptions> optionsAccessor, ILogger<WeedService> logger)
        {
            this.logger = logger;

            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            options = optionsAccessor.Value;

            client.BaseAddress = new Uri(options.Server);
        }

        public async Task<UploadResult> Upload(string name, Stream stream, int uid = 0, int did = 0)
        {
            var result = new UploadResult();
            try
            {
                var ext = Path.GetExtension(name);
                if (!options.Extensions.Contains(ext))
                {
                    result.Error = "不允许上传该类型的文件";
                    return result;
                }
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(stream), name);
                var rep = await client.PostAsync("submit", content);
                var json = await rep.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<UploadResult>(json);
                if (result)
                {
                    result.Uid = uid;
                    result.Did = did;
                    result.FileName = name;
                }
                OnUpload?.Invoke(result);
                return result;
            }
            catch (Exception e)
            {
                result.Error = e.Message;

                logger.LogError("上传异常：" + e.Message);

                return result;
            }

            /*
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

                if (string.IsNullOrEmpty(val) || val.StartsWith("-"))
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
            */
        }


        private void Save(UploadResult o)
        {
            //if (options.EnabledStorage && options.SmsStorage != null)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        try
            //        {
            //            var v = options.SmsStorage.Save(msg);
            //            if (!v)
            //            {
            //                var s = string.Format("唯一识别码{0} 手机号{1} 主题{2} 内容{3}", msg.Id, msg.Mobile, msg.Topic, msg.Message);
            //                logger.LogWarning("写入数据失败：" + s);
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            logger.LogError("写入数据异常：" + e.Message);
            //        }
            //    });
            //}
        }
        //private async Task<string> SendSms(string mobile, string message)
        //{
        //    var dic = new Dictionary<string, string>();
        //    dic.Add("iMobiCount", "1");
        //    dic.Add("pszSubPort", "*");
        //    dic.Add("userId", "J00516");
        //    dic.Add("password", "753357");
        //    dic.Add("pszMobis", mobile);
        //    dic.Add("pszMsg", message);

        //    var content = new FormUrlEncodedContent(dic);
        //    var url = "MWGate/wmgw.asmx/MongateCsSpSendSmsNew";

        //    var response = await client.PostAsync(url, content);

        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        using (var stream = await response.Content.ReadAsStreamAsync())
        //        {
        //            return XElement.Load(stream).Value;
        //        }
        //    }
        //    else
        //    {
        //        return response.ReasonPhrase;
        //    }
        //}
    }
}
