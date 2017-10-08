using System;
using Ayatta.Domain;

namespace Ayatta.OnlinePay
{
    public interface IOnlinePay
    {
        /// <summary>
        /// 支付平台名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 接收到支付平台通知并验证通过后触发 用于处理系统内支付逻辑
        /// </summary>
        Func<Notification, bool> Notified { get; set; }

        /// <summary>
        /// 生成支付所需信息 url json等
        /// </summary>
        /// <param name="payment">付款信息</param>
        /// <param name="exts">其他参数信息</param>
        /// <returns></returns>
        string Pay(Payment payment,params  string[] exts);

        /// <summary>
        /// 处理支付平台通知
        /// </summary>
        /// <param name="param">支付平台通知参数</param>
        /// <returns></returns>
        Result<Notification> HandleNotify(PaymentParam param);
    }

}