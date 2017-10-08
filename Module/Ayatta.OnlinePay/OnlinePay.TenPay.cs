using System;
using Ayatta.Domain;
using System.Xml.Linq;

namespace Ayatta.OnlinePay
{
    /// <summary>
    /// 财富通支付
    /// </summary>
    internal sealed class TenPay : OnlinePay
    {
        /// <summary>
        /// 财富通支付
        /// </summary>
        /// <param name="platform">支付平台配置信息</param>
        public TenPay(PaymentPlatform platform) : base(platform)
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
            //var payNo = payment.PayNo;//支付平台订单号
            var amount = payment.Amount * 100; //转换为分
            var gateway = payment.BankCode;

            //系统可选参数
            param.Add("sign_type", "MD5");
            param.Add("service_version", "1.0");
            param.Add("input_charset", "UTF-8");
            param.Add("sign_key_index", "1");

            //业务参数 是否必填 类型 说明
            param.Add("bank_type", gateway); // 否	String(16)	银行类型，默认为“DEFAULT”－财付通支付中心。银行直连编码及额度请与技术支持联系
            param.Add("body", "test"); // 是	String(32)	商品描述
            param.Add("attach", payment.State); // 否	 String(127)	附加数据，原样返回
            param.Add("return_url", Platform.CallbackUrl); // 是	String(255)	交易完成后跳转的URL，需给绝对路径，255字符内，格式如:http://wap.tenpay.com/tenpay.asp，通过该路径直接将支付结果以Get的方式返回
            param.Add("notify_url", Platform.NotifyUrl); // 是	String(255)	接收财付通通知的URL，需给绝对路径，255字符内，格式如:http://wap.tenpay.com/tenpay.asp
            //param.Add("buyer_id	", ""); // 否	String(64)	买方的财付通账户(QQ 或EMAIL)。若商户没有传该参数，则在财付通支付页面，买家需要输入其财付通账户。
            param.Add("partner", Platform.MerchantId); // 是	String(10)	商户号,由财付通统一分配的10位正整数(120XXXXXXX)号
            param.Add("out_trade_no", payId); // 是	String(32)	商户系统内部的订单号,32个字符内、可包含字母,确保在商户系统唯一
            param.Add("total_fee	", amount.ToString("F0")); // 是	Int	订单总金额，单位为分 
            param.Add("fee_type	", "1"); // 是 Int 现金支付币种,取值：1（人民币）,默认值是1，暂只支持1
            param.Add("spbill_create_ip", payment.IpAddress); // 是 String(15)	订单生成的机器IP，指用户浏览器端IP，不是商户服务器IP 测试时填写127.0.0.1,只能支持10分以下交易

            //业务可选参数
            /*
            param.Add("time_start", ""); //否 String(14)	订单生成时间，格式为yyyyMMddhhmmss，如2009年12月25日9点10分10秒表示为20091225091010。时区为GMT+8 beijing。该时间取自商户服务器
            param.Add("time_expire", ""); // 否	String(14)	订单失效时间，格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。时区为GMT+8 beijing。该时间取自商户服务器
            param.Add("transport_fee", "");  // 否	Int 物流费用，单位为分。如果有值，必须保证transport_fee + product_fee=total_fee
            param.Add("product_fee", "");  // 否 Int 商品费用，单位为分。如果有值，必须保证transport_fee + product_fee=total_fee
            param.Add("goods_tag", "");  // 否	String(32)	商品标记，优惠券时可能用到
            */


            param.Add("sign", CreateSign(param));

            //获取带参数的url
            return Platform.GatewayUrl + param.ToQueryString(true);
        }

        /// <summary>
        /// 处理支付平台通知
        /// </summary>
        /// <param name="param">第三方支付平台通知参数</param>
        /// <returns></returns>
        public override Result<Notification> HandleNotify(PaymentParam param)
        {
            var now = DateTime.Now;
            var input = param.ToXml();
            var notification = new Notification(Platform.Id, input, "fail");
            var result = new Result<Notification>(false, notification);

            var isValid = VerifySign(param);

            if (isValid)
            {
                var reqParam = new PaymentParam();

                var notifyId = param.GetString("notify_id"); // 通知id

                reqParam.Add("notify_id", notifyId);
                reqParam.Add("partner", Platform.MerchantId);
                reqParam.Add("sign", CreateSign(reqParam));

                //系统可选参数
                reqParam.Add("sign_type", "MD5");
                reqParam.Add("service_version", "1.0");
                reqParam.Add("input_charset", "UTF-8");
                reqParam.Add("sign_key_index", "1");

                var url = "https://gw.tenpay.com/gateway/verifynotifyid.xml" + reqParam.ToQueryString(true);

                var xml = XElement.Load(url);
                var notifyParam = new PaymentParam(xml);

                result.Data.PayId = notifyParam.GetString("out_trade_no");//系统支付单号
                result.Data.PayNo = notifyParam.GetString("transaction_id"); // 财付通交易号，28位长的数值，其中前10位为商户号，之后8位为订单产生的日期;
                result.Data.Amount = notifyParam.GetInt("total_fee") / 100M; // 转换为元为单位;
                result.Data.PaidOn = notifyParam.GetDateTime("time_end", now);

                result.Data.State = param.GetString("attach");//扩展字段 支付平台原样返回

                var bankOrderNo = notifyParam.GetString("bank_billno"); // 银行订单号，若为财付通余额支付则为空 

                if (VerifySign(notifyParam) && notifyParam["retcode"] == "0" && notifyParam["trade_state"] == "0" && notifyParam["trade_mode"] == "1")
                {
                    var status = OnNotified(notification);
                    if (status)
                    {
                        result.Status = true;
                        result.Data.SetOutput("success");
                    }
                    else
                    {
                        result.Message = "TenPay OnNotified 返回false";
                    }
                }
                else
                {
                    result.Message = "TenPay 通知验证失败";
                }
            }
            else
            {
                result.Message = "TenPay 签名验证失败";
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
            var temp = new PaymentParam(param);
            //temp.Add("key", "psL6ocgtbjeJKsGb4nslFZiYclpRQ0ik");
            temp.Add("key", Platform.PrivateKey);

            return Hash(temp.ToQueryString(), true);
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="param">待验证签名参数</param>
        /// <returns></returns>
        public bool VerifySign(PaymentParam param)
        {
            var sign = param.GetString("sign");
            param = param.Remove("sign");//移除不参与签名的参数
            return sign.ToUpper().Equals(CreateSign(param));
        }
    }
}
