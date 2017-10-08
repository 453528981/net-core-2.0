
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System;

namespace Ayatta.Nsq
{
    /// <summary>
    /// 
    /// </summary>
    public class NsqOptions : IOptions<NsqOptions>
    {
        /// <summary>
        /// Server
        /// </summary>
        public string Server { get; set; } = "http://127.0.0.1:4151/";

        /// <summary>
        /// ³¬Ê±
        /// </summary>
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0,30);

        //public Action<Message> OnPublished { get; set; }

        NsqOptions IOptions<NsqOptions>.Value => this;
    }
}