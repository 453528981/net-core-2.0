using System;
using ProtoBuf;

namespace Ayatta.Domain
{

    ///<summary>
    /// 商品评价汇总摘要
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ItemComment : IEntity<int>
    {

        #region Properties

        ///<summary>
        /// 商品Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 有晒图的评价总数
        ///</summary>
        public int ImgCount { get; set; }

        ///<summary>
        /// 所有评价总数
        ///</summary>
        public int SumCount { get; set; }

        ///<summary>
        /// 1分评价数
        ///</summary>
        public int CountA { get; set; }

        ///<summary>
        /// 2分评价数
        ///</summary>
        public int CountB { get; set; }

        ///<summary>
        /// 3分评价数
        ///</summary>
        public int CountC { get; set; }

        ///<summary>
        /// 4分评价数
        ///</summary>
        public int CountD { get; set; }

        ///<summary>
        /// 5分评价数
        ///</summary>
        public int CountE { get; set; }

        ///<summary>
        /// 买家印象标签
        ///</summary>
        public string TagData { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        public DateTime ModifiedOn { get; set; }

        #endregion
    }
}
