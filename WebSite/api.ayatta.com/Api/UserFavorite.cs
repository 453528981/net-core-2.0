
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Web.Api
{
    #region 用户收藏列表
    /// <summary>
    /// 用户收藏列表 响应
    /// </summary>
    public sealed class UserFavoriteListResponse : Response
    {
        /// <summary>
        /// 用户收货地址
        /// </summary>
        public IList<UserFavorite> Data { get; set; }

    }

    /// <summary>
    /// 用户收藏列表 请求
    /// </summary>
    public sealed class UserFavoriteListRequest : Request<UserFavoriteListResponse>
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 分组 0商品 2品牌 3店铺
        /// </summary>
        public byte GroupId { get; set; }
    }
    #endregion

    #region 用户收货地址创建
    /// <summary>
    /// 用户收藏创建 响应
    /// </summary>
    public sealed class UserFavoriteCreateResponse : Response
    {
        /// <summary>
        /// 是否创建成功
        /// </summary>
        //public bool Data { get; set; }

    }

    /// <summary>
    /// 用户收藏创建 请求
    /// </summary>
    public sealed class UserFavoriteCreateRequest : Request<UserFavoriteCreateResponse>
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 分组 0商品 2品牌 3店铺
        /// </summary>
        public byte GroupId { get; set; }

        /// <summary>
        /// 商品/品牌/店铺名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品Id 品牌Id 店铺Id
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 扩展信息 商品编号等
        /// </summary>
        public string Extra { get; set; }
    }
    #endregion

    #region 用户收藏删除
    /// <summary>
    /// 用户收藏删除 响应
    /// </summary>
    public sealed class UserFavoriteDeleteResponse : Response
    {
        /// <summary>
        /// 是否删除成功
        /// </summary>
        //public bool Data { get; set; }

    }

    /// <summary>
    /// 用户收藏删除 请求
    /// </summary>
    public sealed class UserFavoriteDeleteRequest : Request<UserFavoriteDeleteResponse>
    {
        /// <summary>
        /// Ids
        /// </summary>
        public int[] Ids { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
    }
    #endregion
}