
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Web.Api
{
    #region 商品咨询列表
    /// <summary>
    /// 商品咨询列表 响应
    /// </summary>
    public sealed class ConsultationListResponse : Response
    {
        /// <summary>
        /// 商品咨询
        /// </summary>
        public IList<Consultation> Data { get; set; }

    }

    /// <summary>
    /// 商品咨询列表 请求
    /// </summary>
    public sealed class ConsultationListRequest : Request<ConsultationListResponse>
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
        /// 分组 0商品咨询 1库存配送 2支付问题 3发票保修
        /// </summary>
        public byte GroupId { get; set; }
    }
    #endregion

    #region 商品咨询创建
    /// <summary>
    /// 商品咨询 响应
    /// </summary>
    public sealed class ConsultationCreateResponse : Response
    {
        /// <summary>
        /// 是否创建成功
        /// </summary>
        public bool Data { get; set; }

    }

    /// <summary>
    /// 商品咨询 请求
    /// </summary>
    public sealed class ConsultationCreateRequest : Request<ConsultationCreateResponse>
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
        /// 分组 0商品咨询 1库存配送 2支付问题 3发票保修
        /// </summary>
        public byte GroupId { get; set; }

        /// <summary>
        /// 咨询
        /// </summary>
        public string Question { get; set; }

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

    #region 商品咨询删除
    /// <summary>
    /// 商品咨询 响应
    /// </summary>
    public sealed class ConsultationDeleteResponse : Response
    {
        /// <summary>
        /// 是否删除成功
        /// </summary>
        public bool Data { get; set; }

    }

    /// <summary>
    /// 商品咨询删除 请求
    /// </summary>
    public sealed class ConsultationDeleteRequest : Request<ConsultationDeleteResponse>
    {
        /// <summary>
        /// 咨询Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
    }
    #endregion
}