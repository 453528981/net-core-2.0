using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// 国家
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Country : IEntity<string>
    {
        #region Properties

        ///<summary>
        /// Id 三位数字代码
        ///</summary>
        public string Id { get; set; }

        ///<summary>
        /// 三位字母代码
        ///</summary>
        public string Code { get; set; }

        ///<summary>
        /// 中文名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// 国旗
        ///</summary>
        public string Flag { get; set; }

        ///<summary>
        /// 英文名称
        ///</summary>
        public string EnName { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        public string Extra { get; set; }

        ///<summary>
        /// 状态 ture可用 false不可用
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
