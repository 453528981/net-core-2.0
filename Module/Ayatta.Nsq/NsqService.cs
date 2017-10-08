using System;
using System.Net;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ayatta.Nsq
{
    public class NsqService : INsqService
    {
        private bool status = true;
        private readonly ILogger logger;
        private readonly NsqOptions options;

        //private readonly Timer timer;
        private readonly HttpClient client;

        private const byte MaxNameLength = 32;
        private const string ValidNameExpr = "[.a-zA-Z0-9_-]";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsAccessor"></param>
        /// <param name="logger"></param>
        public NsqService(IOptions<NsqOptions> optionsAccessor, ILogger<NsqService> logger)
        {
            this.logger = logger;

            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            options = optionsAccessor.Value;

            client = new HttpClient();
            client.Timeout = options.Timeout;
            client.BaseAddress = new Uri(options.Server);

            //timer = new Timer(HealthyCheck, null, 5000, 5000);
        }

        /// <summary>
        /// 向消息队列写入消息
        /// </summary>
        /// <param name="topic">topic</param>
        /// <param name="message">message</param>
        /// <param name="channel">channel</param>
        /// <returns>消息id</returns>
        public string Publish(string topic, IMessage message, string channel = null)
        {
            if (!CheckName(topic))
            {
                throw new ArgumentException("Bad Name", "topic");
            }
            if (message == null)
            {
                throw new ArgumentNullException("message", "Bad Message");
            }

            var id = Guid.NewGuid().ToString();
            var data = JsonConvert.SerializeObject(message);
            Publish(id, topic, channel, data, false);
            return id;
        }

        /// <summary>
        /// 向消息队列写入消息(批量)
        /// </summary>
        /// <param name="topic">topic</param>
        /// <param name="messages">messages</param>
        /// <param name="channel">channel</param>
        /// <returns>消息id</returns>
        public string Publish(string topic, IList<IMessage> messages, string channel)
        {
            if (!CheckName(topic))
            {
                throw new ArgumentException("Bad Name", "topic");
            }

            if (messages == null)
            {
                throw new ArgumentNullException("messages", "Bad Messages");
            }

            var len = messages.Count;
            var sb = new StringBuilder();

            for (var i = 0; i < len; i++)
            {
                var message = messages[i];

                if (i == len - 1)
                {
                    sb.Append(JsonConvert.SerializeObject(message));
                }
                else
                {
                    sb.Append(JsonConvert.SerializeObject(message) + "\n");
                }
            }

            var id = Guid.NewGuid().ToString();

            Publish(id, topic, channel, sb.ToString(), true);
            return id;
        }

        private void Publish(string id, string topic, string channel, string content, bool multiple)
        {
            var path = string.Format("pub?topic={0}", topic);
            if (!string.IsNullOrEmpty(channel))
            {
                path = string.Format("pub?topic={0}&channel={1}", topic, channel);
            }
            if (multiple)
            {
                path = string.Format("mpub?topic={0}", topic);
                if (!string.IsNullOrEmpty(channel))
                {
                    path = string.Format("mpub?topic={0}&channel={1}", topic, channel);
                }
            }
            var endpoint = options.Server + path;
            var message = new Message(id, topic, channel, content, endpoint);
            if (!status)
            {
                message.Status = "服务器不可用";
                Published(message);
                return;
            }
            try
            {
                client.PostAsync(path, new StringContent(content)).ContinueWith(async x =>
                {
                    var v = await x;
                    if (v.StatusCode == HttpStatusCode.OK)
                    {
                        var str = await v.Content.ReadAsStringAsync();

                        message.Status = str;
                    }
                    else
                    {
                        message.Status = v.ReasonPhrase;
                    }
                    Published(message);
                });
            }
            catch (Exception e)
            {
                message.Status = e.Message;
                Published(message);
            }
        }


        private void Published(Message message)
        {
            if (message.Status != "OK")
            {
                logger.LogError("写入消息队列失败 " + message.Content);
            }
            logger.LogInformation("写入消息队列成功 " + message.Content);
        }

        /// <summary>
        /// 检查 topic channel 参数的合法性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool CheckName(string name)
        {
            return name != null && name.Length > 1 && name.Length <= MaxNameLength && System.Text.RegularExpressions.Regex.IsMatch(name, ValidNameExpr);
        }

        private void HealthyCheck(object state)
        {
            client.GetStringAsync("ping").ContinueWith(x =>
            {
                status = x.Result == "OK";
            });
        }
    }
}
