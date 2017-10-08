
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Ayatta.Weed
{
    /// <summary>
    /// 
    /// </summary>
    public class WeedOptions : IOptions<WeedOptions>
    {
        /// <summary>
        /// Server
        /// </summary>
        public string Server { get; set; } = "http://localhost:9333/";

        public uint LimitSize { get; set; } = 1000 * 1024;

        public string[] Extensions { get; set; } = new string[] { ".gif", ".png", ".jpg", ".jpeg" };

        WeedOptions IOptions<WeedOptions>.Value => this;
    }
}