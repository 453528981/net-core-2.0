using Ayatta.Cart;

namespace Ayatta.Api
{
    #region 购物车获取
    /// <summary>
    /// 购物车获取 响应
    /// </summary>
    public class CartGetResponse : Response
    {
        /// <summary>
        /// 购物车信息
        /// </summary>
        public CartData Data { get; set; }
    }

    /// <summary>
    /// 购物车获取 请求
    /// </summary>
    public class CartGetRequest : Request<CartGetResponse>
    {
        /// <summary>
        /// 购物车guid
        /// </summary>
        public string Guid { get; set; }
    }
    #endregion

    #region 购物车操作
    /// <summary>
    /// 购物车操作 响应
    /// </summary>
    public class CartOptResponse : Response
    {
        /// <summary>
        /// 购物车信息
        /// </summary>
        public CartData Data { get; set; }
    }

    /// <summary>
    /// 购物车操作 请求
    /// </summary>
    public class CartOptRequest : Request<CartOptResponse>
    {
        /// <summary>
        /// 购物车guid
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public Operate Opt { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 商品SkuId
        /// </summary>
        public int SkuId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

    }
    #endregion

    #region 购物车选择
    /// <summary>
    /// 购物车选择 响应
    /// </summary>
    public class CartSelectResponse : Response
    {
        /// <summary>
        /// 购物车信息
        /// </summary>
        public CartData Data { get; set; }
    }

    /// <summary>
    /// 购物车选择 请求
    /// </summary>
    public class CartSelectRequest : Request<CartSelectResponse>
    {
        /// <summary>
        /// 购物车guid
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 选择类型
        /// </summary>
        public Select Select { get; set; }

        public int Param { get; set; }

        public bool Selected { get; set; }

    }
    #endregion

    #region 购物车清除
    /// <summary>
    /// 购物车清除 响应
    /// </summary>
    public class CartCleanResponse : Response
    {
        /// <summary>
        /// 购物车信息
        /// </summary>
        public CartData Data { get; set; }
    }

    /// <summary>
    /// 购物车清除 请求
    /// </summary>
    public class CartCleanRequest : Request<CartCleanResponse>
    {
        /// <summary>
        /// 购物车guid
        /// </summary>
        public string Guid { get; set; }

        public int[] Skus { get; set; }
        public int[] Items { get; set; }
        public int[] Packages { get; set; }

        /// <summary>
        /// 是否清除全部
        /// </summary>
        public bool All { get; set; }

    }
    #endregion

    #region 购物车确认
    /// <summary>
    /// 购物车确认 响应
    /// </summary>
    public class CartConfirmResponse : Response
    {
        /// <summary>
        /// 购物车信息
        /// </summary>
        public CartData Data { get; set; }
    }

    /// <summary>
    /// 购物车确认 请求
    /// </summary>
    public class CartConfirmRequest : Request<CartConfirmResponse>
    {
        /// <summary>
        /// 购物车guid
        /// </summary>
        public string Guid { get; set; }
        public int[] Skus { get; set; }
        public int[] Items { get; set; }
        public int[] Packages { get; set; }

        /// <summary>
        /// 用户收货 地址Id
        /// </summary>
        public int AddressId { get; set; }

    }
    #endregion

    #region 购物车提交结算
    /// <summary>
    /// 购物车提交结算 响应
    /// </summary>
    public class CartSubmitResponse : Response
    {
        /// <summary>
        /// 待支付信息
        /// </summary>
        public string Data { get; set; }
    }

    /// <summary>
    /// 购物车提交结算 请求
    /// </summary>
    public class CartSubmitRequest : Request<CartSubmitResponse>
    {
        /// <summary>
        /// 购物车guid
        /// </summary>
        public string Guid { get; set; }

        public int[] Skus { get; set; }
        public int AppPay { get; set; }
        //public int[] Items { get; set; }
        //public int[] Packages { get; set; }
        //public string IpAddress { get; set; }
        //public int UserAddressId { get; set; }


    }
    #endregion
}