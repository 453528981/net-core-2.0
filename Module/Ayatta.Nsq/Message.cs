using System;

namespace Ayatta.Nsq
{
    /// <summary>
    /// 消息基类
    /// </summary>
    public interface IMessage
    {

    }

    public class TestMessage:IMessage
    {
        public string Name { get; set; }
    }

    ///// <summary>
    ///// 简单消息类
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class Message<T> : Message
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public T Data { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Message()
    //        : this(default(T))
    //    {

    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="data"></param>
    //    public Message(T data)
    //    {
    //        Data = data;
    //    }
    //}

    public sealed class Message
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { get; set; }

        public string Topic { get; set; }

        public string Channel { get; set; }

        /// <summary>
        /// 数据 json格式
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息被发送到的URL
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// 状态 ok为发送成功
        /// </summary>
        public string Status { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        internal Message(string id, string topic, string channel, string content, string endpoint)
        {
            Id = id;
            Topic = topic;
            Channel = channel;
            Content = content;
            Endpoint = endpoint;
        }
    }
}
