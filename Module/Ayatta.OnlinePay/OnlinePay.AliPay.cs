using System;
using Ayatta.Domain;

namespace Ayatta.OnlinePay
{
    /// <summary>
    /// 支付宝支付
    /// </summary>
    public sealed class AliPay : OnlinePay
    {
        private static string Charset => "utf-8";

        private static string SignType => "MD5";

        private static string PaymentType => "1";

        private static string SellerEmail => "tt7shop24@126.com";//卖家支付宝账号

        /// <summary>
        /// 支付宝支付
        /// </summary>
        /// <param name="platform">支付平台配置信息</param>
        public AliPay(PaymentPlatform platform) : base(platform)
        {

        }

        /// <summary>
        /// 生成支付所需信息 url json等
        /// </summary>
        /// <param name="payment">付款信息</param>
        /// <param name="exts">其他参数信息</param>
        /// <returns></returns>
        public override string Pay(Payment payment, params string[] exts)
        {
            
            var param = new PaymentParam();

            var payId = payment.Id;//系统订单号
            var amount = payment.Amount.ToString("F2");

            var body = "7shop24Product";
            var showUrl = "http://www.tiantian.com";

            param.Add("partner", Platform.MerchantId);
            param.Add("_input_charset", Charset);
            param.Add("service", "create_direct_pay_by_user");
            param.Add("payment_type", PaymentType);
            param.Add("notify_url", Platform.NotifyUrl);
            param.Add("return_url", Platform.CallbackUrl);
            param.Add("seller_email", SellerEmail);
            param.Add("out_trade_no", payId);
            param.Add("subject", payment.Subject);
            param.Add("total_fee", amount);
            param.Add("body", body);
            param.Add("show_url", showUrl);
            //param.Add("anti_phishing_key", anti_phishing_key);
            param.Add("exter_invoke_ip", payment.IpAddress);
            param.Add("extra_common_param", payment.State);//支付平台原样返回

            param.Add("sign", CreateSign(param));
            param.Add("sign_type", SignType);

            return Platform.GatewayUrl + param.ToQueryString(true, true, true);
        }

        /// <summary>
        /// 处理支付平台通知
        /// 支付宝是用POST方式发送通知信息
        /// </summary>
        /// <param name="param">支付平台通知参数</param>
        /// <returns></returns>
        public override Result<Notification> HandleNotify(PaymentParam param)
        {
            var now = DateTime.Now;
            var input = param.ToXml();
            var notification = new Notification(Platform.Id, input, "fail");
            var result = new Result<Notification>(false, notification);

            OnTraced("支付宝支付 通知数据", input);

            var isValid = VerifySign(param);

            if (isValid)
            {
                result.Data.PayId = param.GetString("out_trade_no"); //商户订单号
                result.Data.PayNo = param.GetString("trade_no");//支付宝交易号
                result.Data.Amount = param.GetDecimal("total_fee");//支付金额
                result.Data.PaidOn = param.GetDateTime("gmt_payment", now); //支付时间

                result.Data.State = param.GetString("extra_common_param");//扩展字段 支付平台原样返回

                var tradeStatus = param.GetString("trade_status");//交易状态

                if (tradeStatus == "TRADE_FINISHED" || tradeStatus == "TRADE_SUCCESS")
                {
                    var status = OnNotified(notification);
                    if (status)
                    {
                        result.Status = true;
                        result.Data.SetOutput("success");

                        OnTraced("支付宝支付 处理通知成功", result.Data.PayId);
                    }
                    else
                    {
                        result.Message = "支付宝支付 处理通知失败 OnNotified 返回 false";

                        OnTraced("支付宝支付 处理通知失败", "OnNotified 返回 false");
                    }
                }
                else
                {
                    result.Message = "支付宝支付 处理通知失败 交易状态错误 " + tradeStatus;

                    OnTraced("支付宝支付 处理通知失败 交易状态错误", tradeStatus);
                }
            }
            else
            {
                result.Message = "支付宝支付 处理通知失败 签名或通知验证失败";

                OnTraced("支付宝支付 处理通知失败 处理通知失败 签名或通知验证失败", string.Empty);
            }
            return result;
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="param">待签名参数</param>
        /// <returns></returns>
        private string CreateSign(PaymentParam param)
        {
            var str = param.ToQueryString(false, true);
            OnTraced("支付宝支付 待签名参数", str);
            var hash = Hash(str + Platform.PrivateKey);
            OnTraced("支付宝支付 签名结果", hash);
            return hash;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="param">待验证签名参数</param>
        /// <returns></returns>
        public bool VerifySign(PaymentParam param)
        {
            var sign = param.GetString("sign");//获取返回时的签名验证结果

            param = param.Remove("sign", "sign_type");//移除不参与签名的参数

            var isValid = sign.Equals(CreateSign(param));

            //验证是否是支付宝服务器发来的请求
            var notifyId = param.GetString("notify_id");
            if (isValid && !string.IsNullOrEmpty(notifyId))
            {
                var reqParam = new PaymentParam();
                reqParam.Add("service", "notify_verify");
                reqParam.Add("partner", Platform.MerchantId);
                reqParam.Add("notify_id", notifyId);
                var url = reqParam.ToQueryString(true, true);
                var responseTxt = Client.GetStringAsync(url).Result;
                return responseTxt.ToLower() == "true";
            }
            return false;
        }
    }
}