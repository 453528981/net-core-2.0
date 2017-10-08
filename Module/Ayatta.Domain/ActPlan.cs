using System;
using ProtoBuf;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    ///<summary>
    /// 官方活动计划
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ActPlan : IEntity<int>
    {
        #region Properties

        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// 标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// 图标
        ///</summary>
        public string Icon { get; set; }

        ///<summary>
        /// 图片
        ///</summary>
        public string Picture { get; set; }

        ///<summary>
        /// 提前预热天数
        ///</summary>
        public int WarmUp { get; set; }

        ///<summary>
        /// 描述
        ///</summary>
        public string Summary { get; set; }

        ///<summary>
        /// 报名开始时间
        ///</summary>
        public DateTime OpendOn { get; set; }

        ///<summary>
        /// 报名结束时间
        ///</summary>
        public DateTime ClosedOn { get; set; }

        ///<summary>
        /// 活动开始时间 必需晚于报名结束时间
        ///</summary>
        public DateTime StartedOn { get; set; }

        ///<summary>
        /// 活动结束时间
        ///</summary>
        public DateTime StoppedOn { get; set; }

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
        public virtual IList<ActItem> Items { get; set; }

        #endregion
    }
}
