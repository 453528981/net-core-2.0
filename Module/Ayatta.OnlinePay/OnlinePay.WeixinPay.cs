using System;
using System.Text;
using Ayatta.Domain;
using System.Net.Http;
using System.Xml.Linq;

namespace Ayatta.OnlinePay
{
    /// <summary>
    /// 微信支付
    /// </summary>
    internal sealed class WeixinPay : OnlinePay
    {
        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="platform">支付平台配置信息</param>
        public WeixinPay(PaymentPlatform platform) : base(platform)
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
            var openId = string.Empty;
            var tradeType = "NATIVE";//取值如下 JSAPI NATIVE APP

            if (exts.Length > 0)
            {
                tradeType = exts[0];
            }
            if (tradeType == "JSAPI")
            {
                if (exts.Length == 2)
                {
                    openId = exts[1]; // rade_type=JSAPI 时此参数必传 用户在商户appid下的唯一标识
                }
                else
                {
                    throw new ArgumentException("当tradeType值为JSAPI时，必需提供openId参数");
                }
            }

            return Pay(payment, tradeType, openId);
        }

        private string Pay(Payment payment, string tradeType, string openId)
        {
            var param = new PaymentParam();

            param.Add("appid", Platform.PublicKey);//微信分配的公众账号ID（企业号corpid即为此appId）
            param.Add("mch_id", Platform.MerchantId);//微信支付分配的商户号
            param.Add("nonce_str", GenerateNonceStr());//随机字符串，不长于32位
            param.Add("body", payment.Subject);//商品简单描述 该字段须严格按照规范传递

            param.Add("attach", payment.State);//附加数据 在查询API和支付通知中原样返回 该字段主要用于商户携带订单的自定义数据
            param.Add("out_trade_no", payment.Id);//商户系统内部的订单号,32个字符内 可包含字母
            param.Add("total_fee", (payment.Amount * 100).ToString("F0"));//订单总金额，单位为分 不能带小数
            param.Add("spbill_create_ip", payment.IpAddress);//APP和网页支付提交用户端ip Native支付填调用微信支付API的机器IP
            param.Add("notify_url", Platform.NotifyUrl);
            param.Add("trade_type", tradeType);
            if (tradeType == "NATIVE")
            {
                param.Add("product_id", payment.Id);// trade_type = NATIVE 时 此参数必传 此id为二维码中包含的商品ID，商户自行定义
            }
            if (tradeType == "JSAPI")
            {
                param.Add("openid", openId);// rade_type=JSAPI 时此参数必传 用户在商户appid下的唯一标识
            }

            param.Add("sign", CreateSign(param));

            PaymentParam data;//支付接口返回参数
            using (Client)
            {
                var xml = param.ToXml();//微信接口接受xml格式的数据

                OnTraced("微信支付 提交数据", xml);

                var hc = new StringContent(xml, Encoding.UTF8, "text/xml");
                var rep = Client.PostAsync(Platform.GatewayUrl, hc).Result;
                var repXml = XElement.Load(rep.Content.ReadAsStreamAsync().Result);

                OnTraced("微信支付 返回数据", repXml.ToString());

                data = new PaymentParam(repXml);
            }

            var s = string.Empty;
            if (data.GetString("return_code") == "SUCCESS" && data.GetString("result_code") == "SUCCESS")
            {
                if (VerifySign(data))
                {
                    if (tradeType == "APP")
                    {
                        var prePayParam = new PaymentParam();
                        var prePayId = data.GetString("prepay_id");

                        prePayParam.Add("appId", Platform.PublicKey);
                        prePayParam.Add("partnerId", Platform.MerchantId);
                        prePayParam.Add("prepayId", prePayId);
                        prePayParam.Add("package", "Sign=WXPay");
                        prePayParam.Add("nonceStr", GenerateNonceStr());
                        prePayParam.Add("timeStamp", GenerateTimeStamp());
                        prePayParam.Add("sign", CreateSign(prePayParam));
                        s = prePayParam.ToJson();
                    }
                    else if (tradeType == "JSAPI")
                    {
                        var prePayParam = new PaymentParam();
                        var prePayId = data.GetString("prepay_id");

                        prePayParam.Add("appId", Platform.PublicKey);
                        prePayParam.Add("timeStamp", GenerateTimeStamp());
                        prePayParam.Add("nonceStr", GenerateNonceStr());
                        prePayParam.Add("package", "prepay_id=" + prePayId);
                        prePayParam.Add("signType", "MD5");
                        prePayParam.Add("paySign", CreateSign(prePayParam));
                        s = prePayParam.ToJson();
                    }
                    else if (tradeType == "NATIVE")
                    {
                        s = data.GetString("code_url");//用于生成微信客户端可识别的支付URL
                    }
                }
                else
                {
                    var err = data.GetString("err_code_des");
                    var msg = string.IsNullOrEmpty(err) ? data.GetString("return_msg") : err;
                    OnTraced("微信支付 验证返回数据签名失败", msg);
                }
            }
            return s;
        }

        /// <summary>
        /// 处理支付平台通知
        /// </summary>
        /// <param name="param">支付平台通知参数</param>
        /// <returns></returns>
        public override Result<Notification> HandleNotify(PaymentParam param)
        {
            var now = DateTime.Now;
            var input = param.ToXml();
            var output = GetOutputXml("FAIL", "FAIL");
            var notification = new Notification(Platform.Id, input, output);
            var result = new Result<Notification>(false, notification);

            OnTraced("微信支付 通知数据", input);

            if (param.GetString("return_code") == "SUCCESS" && param.GetString("result_code") != "SUCCESS")
            {
                if (VerifySign(param))
                {
                    result.Data.PayId = param.GetString("out_trade_no"); //商户订单号
                    result.Data.PayNo = param.GetString("transaction_id"); //微信支付订单号
                    result.Data.Amount = param.GetInt("total_fee") / 100m; //订单总金额，单位为分
                    result.Data.PaidOn = now; //支付完成时间 微信返回的格式不好处理 time_end 格式为yyyyMMddHHmmss 如2009年12月25日9点10分10秒表示为20091225091010

                    result.Data.State = param.GetString("attach"); //支付平台原样返回

                    var status = OnNotified(notification);
                    if (status)
                    {
                        result.Status = true;
                        result.Data.SetOutput(GetOutputXml("SUCCESS", "OK"));

                        OnTraced("微信支付 处理通知成功", result.Data.PayId);
                    }
                    else
                    {
                        result.Message = "微信支付 处理通知失败 OnNotified 返回 false";

                        OnTraced("微信支付 处理通知失败", "OnNotified 返回 false");
                    }
                }
                else
                {
                    var err = param.GetString("err_code_des");
                    var msg = string.IsNullOrEmpty(err) ? param.GetString("return_msg") : err;
                    result.Message = "微信支付 处理通知失败 验证返回数据签名失败 " + msg;

                    OnTraced("微信支付 处理通知失败 验证返回数据签名失败", msg);
                }
            }
            else
            {
                var msg = param.GetString("return_msg");
                result.Message = "微信支付 处理通知失败 接口返回数据信息 " + msg;

                OnTraced("微信支付 处理通知失败 接口返回数据信息", msg);
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
            var str = param.ToQueryString(false, true);//去掉参数值为空的参数
            OnTraced("微信支付 待签名参数", str);

            str = str + "&key=" + Platform.PrivateKey;// 微信签名方式
            var hash = Hash(str, true); //生成签名并转大写

            OnTraced("微信支付 签名结果", hash);
            return hash;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="param">待验证签名参数</param>
        /// <returns></returns>
        public bool VerifySign(PaymentParam param)
        {
            var sign = param.GetString("sign");
            param = param.Remove("sign", "paySign");//移除不参与签名的参数
            return sign.ToUpper().Equals(CreateSign(param));
        }

        private static string GenerateTimeStamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ts.TotalSeconds.ToString("0");
        }

        private static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString("N");
        }

        private static string GetOutputXml(string code, string message)
        {
            var tpl = "<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>";
            return string.Format(tpl, code, message);
        }

        /*
        /// <summary>
        /// 生成Native支付请求Url 
        /// 须在公众平台后台设置支付回调URL 
        /// 接收用户扫码后微信支付系统回调的productid和openid
        /// </summary>
        /// <param name="payment">付款信息</param>
        /// <returns></returns>
        public string GetNativePayUrl(Payment payment)
        {
            var param = new PaymentParam();

            param.Add("appid", Platform.PublicKey);
            param.Add("mch_id", Platform.MerchantId);
            param.Add("time_stamp", GenerateTimeStamp());
            param.Add("nonce_str", GenerateNonceStr());
            param.Add("product_id", payment.Id);
            param.Add("sign", CreateSign(param));
            return "weixin://wxpay/bizpayurl?" + param.ToQueryString();
        }
        */
    }
}
