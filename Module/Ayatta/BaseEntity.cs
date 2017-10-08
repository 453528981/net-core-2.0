using System;

namespace Ayatta
{
    public abstract class BaseEntity<T> : IEntity<T>
    {
        /// <summary>
        /// Id
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// 创建日期时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        
        /// <summary>
        /// 最后一次编辑者
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 最后一次编辑时间
        /// </summary>
        public DateTime ModifiedOn { get; set; }

    }
}