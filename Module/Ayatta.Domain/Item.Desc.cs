using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// 商品描述
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ItemDesc : IEntity<int>
    {
        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 商品详情
        ///</summary>
        public string Detail { get; set; }

        ///<summary>
        /// 使用指南 usage为mysql关键字无法使用
        ///</summary>
        public string Manual { get; set; }

        ///<summary>
        /// 产品实拍
        ///</summary>
        public string Photo { get; set; }

        ///<summary>
        /// 品牌故事
        ///</summary>
        public string Story { get; set; }

        ///<summary>
        /// 使用须知
        ///</summary>
        public string Notice { get; set; }

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
    }

}
