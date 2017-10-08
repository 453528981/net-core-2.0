using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// 帮助
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Help : IEntity<int>
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
        /// 全路径
        ///</summary>
        public string Path { get; set; }

        ///<summary>
        /// 深度
        ///</summary>
        public int Depth { get; set; }

        ///<summary>
        /// 分组Id
        ///</summary>
        public int GroupId { get; set; }

        ///<summary>
        /// 导航URL
        ///</summary>
        public string Link { get; set; }

        ///<summary>
        /// 标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// 摘要
        ///</summary>
        public string Summary { get; set; }

        ///<summary>
        /// 内容
        ///</summary>
        public string Content { get; set; }

        ///<summary>
        /// 排序优先级 从小到大
        ///</summary>
        public int Priority { get; set; }

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
    }

}