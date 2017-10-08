using System;
using ProtoBuf;
using System.Collections.Generic;
using System.Linq;

namespace Ayatta.Domain
{
    ///<summary>
    /// 广告模块
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class AdModule : IEntity<int>
    {
        #region Properties

        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 父Id
        ///</summary>
        public int Pid { get; set; }

        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// 标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// 全路径
        ///</summary>
        public string Path { get; set; }

        ///<summary>
        /// 深度
        ///</summary>
        public int Depth { get; set; }

        ///<summary>
        /// 图标
        ///</summary>
        public string Icon { get; set; }

        ///<summary>
        /// 图片
        ///</summary>
        public string Picture { get; set; }

        ///<summary>
        /// 描述
        ///</summary>
        public string Summary { get; set; }

        ///<summary>
        /// 排序优先级
        ///</summary>
        public int Priority { get; set; }

        ///<summary>
        /// 徽章 标记
        ///</summary>
        public string Badge { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        public string Extra { get; set; }

        ///<summary>
        /// 状态 true可用 false不可用
        ///</summary>
        public bool Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// 条目
        /// </summary>
        public virtual IList<AdItem> Items { get; set; }

        #endregion       
        
    }

    public static partial class Extension
    {

    }
}
