using System;
using ProtoBuf;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    ///<summary>
    /// Weed 目录
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class WeedDir : IEntity<int>
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
        /// 深度
        ///</summary>
        public byte Depth { get; set; }

        ///<summary>
        /// 标记
        ///</summary>
        public string Badge { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        public string Extra { get; set; }

        ///<summary>
        /// 用户id
        ///</summary>
        public int UserId { get; set; }

        ///<summary>
        /// 状态 true可用 false不可用
        ///</summary>
        public bool Status { get; set; }

        ///<summary>
        /// 创建者
        ///</summary>
        public string CreatedBy { get; set; }

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
        /// 文件
        /// </summary>
        public virtual IList<WeedFile> Files { get; set; }

        #endregion
    }
}
