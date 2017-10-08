
namespace Ayatta.Domain
{
    /// <summary>
    /// 促销 http://bbs.taobao.com/catalog/thread/16543510-264265243.htm
    /// </summary>
    public static partial class Promotion
    {
        /// <summary>
        /// 促销类型
        /// </summary>
        public enum Type : byte
        {
            /// <summary>
            /// 无
            /// </summary>
            None = 0,

            /// <summary>
            /// 特价
            /// </summary>            
            A,

            /// <summary>
            /// 店铺优惠
            /// </summary>
            B,

            /// <summary>
            /// 搭配组合套餐
            /// </summary>
            C,

            /// <summary>
            /// 购物车促销
            /// </summary>
            D
        }

        /// <summary>
        /// 促销活动用户参与限制
        /// </summary>
        public enum LimitType : byte
        {
            /// <summary>
            /// 无限制
            /// </summary>

            None = 0,

            /// <summary>
            /// N次
            /// </summary>
            NTimesOnly = 1,

            /// <summary>
            /// 每用户N次
            /// </summary>
            NTimesPerUser = 2
        }

        /// <summary>
        /// 特价类型 A打折  B减价  C促销价
        /// </summary>
        public enum SpecialPriceType : byte
        {
            /// <summary>
            /// 打折
            /// </summary>
            A = 1,

            /// <summary>
            /// 减价
            /// </summary>
            B = 2,

            /// <summary>
            /// 促销价
            /// </summary>
            C = 3
        }

        /// <summary>
        /// 促销折扣/减免金额作用于 订单总金额 商品总金额 运费 税费 商品价格
        /// </summary>
        public enum DiscountOn : byte
        {
            /// <summary>
            /// 默认值
            /// </summary>
            None = 0,

            /// <summary>
            /// 订单总金额(商品总金额+运费+税费-折扣金额)
            /// </summary>
            OrderTotal = 1,

            /// <summary>
            /// 订单商品总金额
            /// </summary>
            OrderSubTotal = 2,

            /// <summary>
            /// 运费
            /// </summary>
            Freight = 3,

            /// <summary>
            /// 税费
            /// </summary>
            Tax = 4,

            /// <summary>
            /// 商品价格
            /// </summary>
            Price = 5,

        }

        /// <summary>
        /// 购物车促销规则计算方式
        /// </summary>
        public enum CalcType : byte
        {
            /// <summary>
            /// 订单总量/金额
            /// </summary>
            A,

            /// <summary>
            /// 已购买过
            /// </summary>
            B,

            /// <summary>
            /// 购物车中包含
            /// </summary>
            C,

            /// <summary>
            /// 有效评论/晒单
            /// </summary>
            D,

            /// <summary>
            /// 有效邀请人
            /// </summary>
            E
        }
    }
}