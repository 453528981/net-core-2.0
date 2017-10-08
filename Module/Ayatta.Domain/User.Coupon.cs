using System;

namespace Ayatta.Domain
{
   
        /// <summary>
        /// 用户优惠券
        /// </summary>
        public class UserCoupon : BaseEntity<int>
        {
            /// <summary>
            /// 是否为店铺优惠券 true为店铺优惠券 false为商品优惠券
            /// </summary>
            public bool Global { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 面额 3 5 10 20 50 100 200
            /// </summary>
            public decimal Amount { get; set; }

            /// <summary>
            /// 有效期开始时间
            /// </summary>
            public DateTime StartOn { get; set; }

            /// <summary>
            /// 有效期结束时间
            /// </summary>
            public DateTime StopOn { get; set; }

            /// <summary>
            /// 适用平台
            /// </summary>
            public Platform Platform { get; set; }

            /// <summary>
            /// 促销优惠券Id Promotion.Coupon.Id
            /// </summary>
            public int OutId { get; set; }

            /// <summary>
            /// 用户Id
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// 卖家Id
            /// </summary>
            public int SellerId { get; set; }

            /// <summary>
            /// 状态
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// 是否有效
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                var now = DateTime.Now;
                return Status && StartOn < now && now < StopOn;
            }

        }
}