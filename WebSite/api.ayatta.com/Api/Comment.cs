
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Web.Api
{
    #region 商品评论列表
    /// <summary>
    /// 商品评论列表 响应
    /// </summary>
    public sealed class CommentListResponse : Response
    {
        /// <summary>
        /// 商品评论
        /// </summary>
        public IList<Comment> Data { get; set; }

        /// <summary>
        /// 商品评论摘要
        /// </summary>
        public ItemComment Summary { get; set; }

    }

    /// <summary>
    /// 商品评论列表 请求
    /// </summary>
    public sealed class CommentListRequest : Request<CommentListResponse>
    {
        /// <summary>
        /// 商品SkuId
        /// </summary>
        public int SkuId { get; set; }

        /// <summary>
        /// 商品ItemId
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 评分 1-5
        /// </summary>
        public byte Score { get; set; }
    }
    #endregion

    #region 商品评论创建
    /// <summary>
    /// 商品评论 响应
    /// </summary>
    public sealed class CommentCreateResponse : Response
    {
        /// <summary>
        /// 是否创建成功
        /// </summary>
        public bool Data { get; set; }

    }

    /// <summary>
    /// 商品评论 请求
    /// </summary>
    public sealed class CommentCreateRequest : Request<CommentCreateResponse>
    {
        /// <summary>
        /// 商品ItemId
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 商品SkuId
        /// </summary>
        public int SkuId { get; set; }

        /// <summary>
        /// 商品销售属性
        /// </summary>
        public string SkuProp { get; set; }

        /// <summary>
        /// 评分 1-5
        /// </summary>
        public byte Score { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserNickname { get; set; }

        /// <summary>
        /// 卖家Id
        /// </summary>
        public int SellerId { get; set; }
    }
    #endregion

    #region 商品评论删除
    /// <summary>
    /// 商品评论 响应
    /// </summary>
    public sealed class CommentDeleteResponse : Response
    {
        /// <summary>
        /// 是否删除成功
        /// </summary>
        public bool Data { get; set; }

    }

    /// <summary>
    /// 商品评论删除 请求
    /// </summary>
    public sealed class CommentDeleteRequest : Request<CommentDeleteResponse>
    {
        /// <summary>
        /// 评论Ids
        /// </summary>
        public int[] Ids { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
    }
    #endregion
}