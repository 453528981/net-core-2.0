using System;
using System.IO;
using System.Threading.Tasks;

namespace Ayatta.Nsq
{
    public interface INsqService
    {
        //Action<Message> OnPublished { get; set; }

        /// <summary>
        /// 向消息队列写入消息
        /// </summary>
        /// <param name="topic">topic</param>
        /// <param name="message">message</param>
        /// <param name="channel">channel</param>
        /// <returns>消息id</returns>
        string Publish(string topic, IMessage message, string channel = null);
    }
}
