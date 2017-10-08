using System;

namespace Ayatta
{
    public abstract class VersionEntity<T> : BaseEntity<T>
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public byte[] Version { get; set; }       

    }
}