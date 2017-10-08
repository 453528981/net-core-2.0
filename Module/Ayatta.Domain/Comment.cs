using System;
using ProtoBuf;
using System.Collections.Generic;


namespace Ayatta.Domain
{
    ///<summary>
    /// 商品评价详情
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Comment : IEntity<int>
    {
        #region Properties

        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 评分 1-5
        ///</summary>
        public byte Score { get; set; }

        ///<summary>
        /// 内容
        ///</summary>
        public string Content { get; set; }

        ///<summary>
        /// 订单Id
        ///</summary>
        public string OrderId { get; set; }

        ///<summary>
        /// 商品Id
        ///</summary>
        public int ItemId { get; set; }

        ///<summary>
        /// 商品图片
        ///</summary>
        public string ItemImg { get; set; }

        ///<summary>
        /// 商品名称
        ///</summary>
        public string ItemName { get; set; }

        ///<summary>
        /// 商品SkuId
        ///</summary>
        public int SkuId { get; set; }

        ///<summary>
        /// 商品销售属性
        ///</summary>
        public string SkuProp { get; set; }

        ///<summary>
        /// 买家印象标签 多个以","分隔
        ///</summary>
        public string TagData { get; set; }

        ///<summary>
        /// 晒图 多个以","分隔
        ///</summary>
        public string ImageData { get; set; }

        ///<summary>
        /// 排序优先级
        ///</summary>
        public int Priority { get; set; }

        ///<summary>
        /// 该评价被赞成总数
        ///</summary>
        public int LikeCount { get; set; }

        ///<summary>
        /// 该评价被回复总数
        ///</summary>
        public int ReplyCount { get; set; }

        ///<summary>
        /// 奖励积分
        ///</summary>
        public int RewardPoint { get; set; }

        ///<summary>
        /// 卖家回复
        ///</summary>
        public string Reply { get; set; }

        ///<summary>
        /// 卖家回复时间
        ///</summary>
        public DateTime ReplyTime { get; set; }

        ///<summary>
        /// 买家追加评价
        ///</summary>
        public string Append { get; set; }

        ///<summary>
        /// 买家追加评价时间
        ///</summary>
        public DateTime AppendTime { get; set; }

        ///<summary>
        /// 买家Id
        ///</summary>
        public int UserId { get; set; }

        ///<summary>
        /// 买家用户名
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        /// 卖家Id
        ///</summary>
        public int SellerId { get; set; }

        ///<summary>
        /// 卖家用户名
        ///</summary>
        public string SellerName { get; set; }

        ///<summary>
        /// 0待审核 1审核未通过 2通过 3积分已返还
        ///</summary>
        public byte Status { get; set; }

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

        #endregion

        [ProtoIgnore]
        public IDictionary<string, string> Dic { get; set; }

        [ProtoIgnore]
        public virtual IList<CommentReply> Replies { get; set; }
    }


    /// <summary>
    /// 商品评价回复
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CommentReply : IEntity<int>
    {
        #region Properties

        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 父级Id
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
        /// 商品Id
        ///</summary>
        public int ItemId { get; set; }

        ///<summary>
        /// 评价详情Id
        ///</summary>
        public int CommentId { get; set; }

        ///<summary>
        /// 内容
        ///</summary>
        public string Content { get; set; }

        ///<summary>
        /// 买家Id
        ///</summary>
        public int UserId { get; set; }

        ///<summary>
        /// 买家用户名
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        /// 状态 true显示 false不显示
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

        #endregion

    }

}
