using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// 商品咨询
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Consultation : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 商品SkuId
        ///</summary>
        public int SkuId { get; set; }

        ///<summary>
        /// 商品Id
        ///</summary>
        public int ItemId { get; set; }

        ///<summary>
        /// 分组 0商品咨询 1库存配送 2支付问题 3发票保修
        ///</summary>
        public byte GroupId { get; set; }

        ///<summary>
        /// 用户Id
        ///</summary>
        public int UserId { get; set; }

        ///<summary>
        /// 用户
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        /// 咨询内容
        ///</summary>
        public string Question { get; set; }

        ///<summary>
        /// 回复
        ///</summary>
        public string Reply { get; set; }

        ///<summary>
        /// 回复处理标识
        ///</summary>
        public byte ReplyFlag { get; set; }

        ///<summary>
        /// 回复者
        ///</summary>
        public string Replier { get; set; }

        ///<summary>
        /// 回复时间
        ///</summary>
        public DateTime RepliedOn { get; set; }

        ///<summary>
        /// 卖家Id
        ///</summary>
        public int SellerId { get; set; }

        ///<summary>
        /// 卖家
        ///</summary>
        public string SellerName { get; set; }

        ///<summary>
        /// 有用数
        ///</summary>
        public int Useful { get; set; }

        ///<summary>
        /// 状态 0未处理 1已回复
        ///</summary>
        public bool Status { get; set; }

        ///<summary>
        /// 来源 pc wap iphone android
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

    }

}
