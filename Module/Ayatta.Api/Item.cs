
using Ayatta.Domain;

namespace Ayatta.Api
{
    #region 商品获取
    /// <summary>
    /// 商品获取 响应
    /// </summary>
    public sealed class ItemGetResponse : Response
    {
        /// <summary>
        /// 商品
        /// </summary>
        public Item Data { get; set; }       

    }

    /// <summary>
    /// 商品获取 请求
    /// </summary>
    public sealed class ItemGetRequest : Request<ItemGetResponse>
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 是否包含该商品下的Sku集合
        /// </summary>
        public bool IncludeSkus { get; set; }
    }
    #endregion
}