using System;
using System.IO;
using System.Text;
using Ayatta.Domain;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Ayatta.OnlinePay
{
    /// <summary>
    /// 支付宝支付
    /// </summary>
    internal sealed class AliAppPay : OnlinePay
    {
        private static string Charset => "utf-8";

        private static string SignType => "RSA";//app支付 固定取值为RSA

        private static string PaymentType => "1"; //支付类型 默认值为 1 商品购买


        /// <summary>
        /// 支付宝支付
        /// </summary>
        /// <param name="platform">支付平台配置信息</param>
        public AliAppPay(PaymentPlatform platform) : base(platform)
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
            //测试 正式删除 开始
            payment.Id = "X" + getRandomValidate(8);//"5950483";
            payment.Subject = "test";
            payment.Amount = 0.01m;
            Platform.MerchantId = "2088501709692126";
            Platform.NotifyUrl = "http://shopping.tiantian.com/ToPay/MobilePayMent/APPPay/ZhiFuBaoCallBack.aspx";

            var payId = payment.Id;//系统订单号
            var amount = payment.Amount.ToString("F2");

            var body = "test-body";
            //测试 正式删除 结束

            var param = new Dictionary<string, string>();

            // 支付宝支付接口必需参数 参数顺序必需固定且与react native里android一致 因为签名原因
            param.Add("service", "mobile.securitypay.pay");//接口名称，固定值
            param.Add("partner", Platform.MerchantId);
            param.Add("_input_charset", Charset);

            param.Add("notify_url", Platform.NotifyUrl);
            param.Add("out_trade_no", payId);
            param.Add("subject", payment.Subject);//商品的标题/交易标题/订单标题/订单关键字等。该参数最长为128个汉字。
            param.Add("payment_type", PaymentType);
            param.Add("seller_id", Platform.MerchantId);//卖家支付宝账号（邮箱或手机号码格式）或其对应的支付宝唯一用户号（以2088开头的纯16位数字）
            param.Add("total_fee", amount);
            param.Add("body", body);//对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body

            //connect = "partner=2088501709692126&seller_id=tt7shop24@126.com&out_trade_no=5950483&subject=订单编号5950483&body=订单编号5950483&total_fee=17.9&notify_url=http://shopping.tiantian.com/ToPay/MobilePayMent/APPPay/ZhiFuBaoCallBack.aspx&service=mobile.securitypay.pay&_input_charset=utf-8&payment_type=1&return_url=http://www.tiantian.com&it_b_pay=1d&show_url=http://www.tiantian.com";

            var sign = UrlEncoder.Default.Encode(CreateSign(param));

            var output = new PaymentParam();

            output.Add("partner", Platform.MerchantId);
            output.Add("seller", Platform.MerchantId);
            output.Add("outTradeNO", payId);
            output.Add("subject", payment.Subject);
            output.Add("body", body);
            output.Add("totalFee", amount);
            output.Add("notifyURL", Platform.NotifyUrl);
            output.Add("sign", sign);
            return output.ToJson();//生成适用于react-native json格式的数据


            /* 2.0
            // 支付宝支付接口必需参数
            param.Add("app_id", "2015010900024452");
            param.Add("method", "alipay.trade.app.pay");
            param.Add("charset", Charset);
            param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            param.Add("version", "1.0");
            param.Add("notify_url", Platform.NotifyUrl);


            var biz = new PaymentParam();
            biz.Add("out_trade_no", payment.Id);
            biz.Add("subject", payment.Subject);
            biz.Add("body", body);
            biz.Add("total_amount", amount);
            biz.Add("product_code", "QUICK_MSECURITY_PAY");

            param.Add("biz_content", biz.ToJson());

            var sign = CreateSign(param);

            param.Add("sign", sign);
            param.Add("sign_type", SignType);


            var output = param.ToQueryString(false, true, true);
            return output;
            */


            /*
            param.Add("privateKey", Platform.PrivateKey);
            param.Add("partner", Platform.MerchantId);
            param.Add("seller", SellerId);
            param.Add("outTradeNO", payId);
            param.Add("body", body);
            param.Add("totalFee", amount);
            param.Add("notifyURL", Platform.NotifyUrl);
            param.Add("itBPay", "30m");//设置未付款交易的超时时间，一旦超时，该笔交易就会自动被关闭。当用户输入支付密码、点击确认付款后（即创建支付宝交易后）开始计时。取值范围：1m～15d，或者使用绝对时间（示例格式：2014-06-13 16:00:00）。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。该参数数值不接受小数点，如1.5h，可转换为90m。
            return param.ToJson();//生成适用于https://github.com/huangzuizui/rn-alipay json格式的数据
            */

        }
        public static string getRandomValidate(int len)
        {
            var ran = new Random();
            string rtuStr = "";
            for (int i = 0; i < len; i++)
            {
                int t = ran.Next(1, 10);

                rtuStr += t.ToString();
            }
            return rtuStr;
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
                result.Data.PaidOn = param.GetDateTime("gmt_payment", now); //该笔交易的买家付款时间。格式为yyyy-MM-dd HH:mm:ss

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
        private string CreateSign(IDictionary<string, string> param)
        {
            //https://doc.open.alipay.com/doc2/detail?treeId=59&articleId=103927&docType=1
            //var str = param.ToQueryString(false, true);
            var sb = new StringBuilder();
            foreach (var o in param)
            {
                sb.AppendFormat("{0}=\"{1}\"&", o.Key, o.Value);
            }
            var str = sb.ToString().TrimEnd('&');
            OnTraced("支付宝支付 待签名参数", str);
            var hash = Hash(str);
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

            var isValid = RSA.Verify(CreateSign(param), sign, Platform.PublicKey, Charset);

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


        private string Hash(string content)
        {
            return RSA.Sign(content, Platform.PrivateKey, Charset);
        }
        /*
        / <summary>
        / 类名：RSAFromPkcs8
        / 功能：RSA解密、签名、验签
        / 详细：该类对Java生成的密钥进行解密和签名以及验签专用类，不需要修改
        / 版本：1.0
        / 修改日期：2016-06-06
        / 说明：
        / 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
        / 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
        / </summary>
        public sealed class RSA
        {
            / <summary>
            / 签名
            / </summary>
            / <param name="content">需要签名的内容</param>
            / <param name="privateKey">私钥</param>
            / <param name="input_charset">编码格式</param>
            / <returns></returns>
            public static string Sign(string content, string privateKey, string input_charset)
            {
                return RSASignCharSet(content, privateKey, input_charset);
            }

            public static string RSASignCharSet(string data, string privateKey, string charset)
            {
                RSACryptoServiceProvider rsaCsp = LoadCertificateString(privateKey);
                byte[] dataBytes = null;
                if (string.IsNullOrEmpty(charset))
                {
                    dataBytes = Encoding.UTF8.GetBytes(data);
                }
                else
                {
                    dataBytes = Encoding.GetEncoding(charset).GetBytes(data);
                }

                byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");

                return Convert.ToBase64String(signatureBytes);
            }

            private static RSACryptoServiceProvider LoadCertificateString(string strKey)
            {
                byte[] data = null;
                读取带
                ata = Encoding.Default.GetBytes(strKey);
                data = Convert.FromBase64String(strKey);
                data = GetPem("RSA PRIVATE KEY", data);
                try
                {
                    RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(data);
                    return rsa;
                }
                catch (Exception ex)
                {
                        throw new AopException("EncryptContent = woshihaoren,zheshiyigeceshi,wanerde", ex);
                }
                return null;
            }

            private static RSACryptoServiceProvider LoadCertificateFile(string filename)
            {
                using (System.IO.FileStream fs = System.IO.File.OpenRead(filename))
                {
                    byte[] data = new byte[fs.Length];
                    byte[] res = null;
                    fs.Read(data, 0, data.Length);
                    if (data[0] != 0x30)
                    {
                        res = GetPem("RSA PRIVATE KEY", data);
                    }
                    try
                    {
                        RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(res);
                        return rsa;
                    }
                    catch (Exception ex)
                    {
                    }
                    return null;
                }
            }



            private static byte[] GetPem(string type, byte[] data)
            {
                string pem = Encoding.UTF8.GetString(data);
                string header = String.Format("-----BEGIN {0}-----\\n", type);
                string footer = String.Format("-----END {0}-----", type);
                int start = pem.IndexOf(header) + header.Length;
                int end = pem.IndexOf(footer, start);
                string base64 = pem.Substring(start, (end - start));

                return Convert.FromBase64String(base64);
            }



            / <summary>
            / 验证签名
            / </summary>
            / <param name="content">需要验证的内容</param>
            / <param name="signedString">签名结果</param>
            / <param name="publicKey">公钥</param>
            / <param name="input_charset">编码格式</param>
            / <returns></returns>
            public static bool Verify(string content, string signedString, string publicKey, string input_charset)
            {
                bool result = false;

                Encoding code = Encoding.GetEncoding(input_charset);
                byte[] Data = code.GetBytes(content);
                byte[] data = Convert.FromBase64String(signedString);
                RSAParameters paraPub = ConvertFromPublicKey(publicKey);
                RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
                rsaPub.ImportParameters(paraPub);

                SHA1 sh = new SHA1CryptoServiceProvider();
                result = rsaPub.VerifyData(Data, sh, data);
                return result;
            }

            / <summary>
            / 用RSA解密
            / </summary>
            / <param name="resData">待解密字符串</param>
            / <param name="privateKey">私钥</param>
            / <param name="input_charset">编码格式</param>
            / <returns>解密结果</returns>
            public static string decryptData(string resData, string privateKey, string input_charset)
            {
                byte[] DataToDecrypt = Convert.FromBase64String(resData);
                List<byte> result = new List<byte>();

                for (int j = 0; j < DataToDecrypt.Length / 128; j++)
                {
                    byte[] buf = new byte[128];
                    for (int i = 0; i < 128; i++)
                    {
                        buf[i] = DataToDecrypt[i + 128 * j];
                    }
                    result.AddRange(decrypt(buf, privateKey, input_charset));
                }
                byte[] source = result.ToArray();
                char[] asciiChars = new char[Encoding.GetEncoding(input_charset).GetCharCount(source, 0, source.Length)];
                Encoding.GetEncoding(input_charset).GetChars(source, 0, source.Length, asciiChars, 0);
                return new string(asciiChars);
            }

            private static byte[] decrypt(byte[] data, string privateKey, string input_charset)
            {
                RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
                SHA1 sh = new SHA1CryptoServiceProvider();
                return rsa.Decrypt(data, false);
            }

            / <summary>
            / 解析java生成的pem文件私钥
            / </summary>
            / <param name="pemstr"></param>
            / <returns></returns>
            private static RSACryptoServiceProvider DecodePemPrivateKey(String pemstr)
            {
                byte[] pkcs8privatekey;
                pkcs8privatekey = Convert.FromBase64String(pemstr);
                if (pkcs8privatekey != null)
                {

                    RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8privatekey);
                    return rsa;
                }
                else
                    return null;
            }

            private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
            {

                byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
                byte[] seq = new byte[15];

                MemoryStream mem = new MemoryStream(pkcs8);
                int lenstream = (int)mem.Length;
                BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
                byte bt = 0;
                ushort twobytes = 0;

                try
                {

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;


                    bt = binr.ReadByte();
                    if (bt != 0x02)
                        return null;

                    twobytes = binr.ReadUInt16();

                    if (twobytes != 0x0001)
                        return null;

                    seq = binr.ReadBytes(15);       //read the Sequence OID
                    if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x04) //expect an Octet string 
                        return null;

                    bt = binr.ReadByte();       //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                    if (bt == 0x81)
                        binr.ReadByte();
                    else
                        if (bt == 0x82)
                        binr.ReadUInt16();
                    ------ at this stage, the remaining sequence should be the RSA private key

                    byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                    RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                    return rsacsp;
                }

                catch (Exception)
                {
                    return null;
                }

                finally { binr.Close(); }

            }


            private static bool CompareBytearrays(byte[] a, byte[] b)
            {
                if (a.Length != b.Length)
                    return false;
                int i = 0;
                foreach (byte c in a)
                {
                    if (c != b[i])
                        return false;
                    i++;
                }
                return true;
            }

            private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
            {
                byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

                 ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
                MemoryStream mem = new MemoryStream(privkey);
                BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
                byte bt = 0;
                ushort twobytes = 0;
                int elems = 0;
                try
                {
                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes != 0x0102) //version number
                        return null;
                    bt = binr.ReadByte();
                    if (bt != 0x00)
                        return null;


                    ------  all private key components are Integer sequences ----
                    elems = GetIntegerSize(binr);
                    MODULUS = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    E = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    D = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    P = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    Q = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    DP = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    DQ = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    IQ = binr.ReadBytes(elems);

                     ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                    RSAParameters RSAparams = new RSAParameters();
                    RSAparams.Modulus = MODULUS;
                    RSAparams.Exponent = E;
                    RSAparams.D = D;
                    RSAparams.P = P;
                    RSAparams.Q = Q;
                    RSAparams.DP = DP;
                    RSAparams.DQ = DQ;
                    RSAparams.InverseQ = IQ;
                    RSA.ImportParameters(RSAparams);
                    return RSA;
                }
                catch (Exception)
                {
                    return null;
                }
                finally { binr.Close(); }
            }

            private static int GetIntegerSize(BinaryReader binr)
            {
                byte bt = 0;
                byte lowbyte = 0x00;
                byte highbyte = 0x00;
                int count = 0;
                bt = binr.ReadByte();
                if (bt != 0x02)     //expect integer
                    return 0;
                bt = binr.ReadByte();

                if (bt == 0x81)
                    count = binr.ReadByte();    // data size in next byte
                else
                    if (bt == 0x82)
                {
                    highbyte = binr.ReadByte();	// data size in next 2 bytes
                    lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    count = bt;		// we already have the data size
                }



                while (binr.ReadByte() == 0x00)
                {   //remove high order zeros in data
                    count -= 1;
                }
                binr.BaseStream.Seek(-1, SeekOrigin.Current);       //last ReadByte wasn't a removed zero, so back up a byte
                return count;
            }

            #region 解析.net 生成的Pem
            private static RSAParameters ConvertFromPublicKey(string pemFileConent)
            {

                byte[] keyData = Convert.FromBase64String(pemFileConent);
                if (keyData.Length < 162)
                {
                    throw new ArgumentException("pem file content is incorrect.");
                }
                byte[] pemModulus = new byte[128];
                byte[] pemPublicExponent = new byte[3];
                Array.Copy(keyData, 29, pemModulus, 0, 128);
                Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
                RSAParameters para = new RSAParameters();
                para.Modulus = pemModulus;
                para.Exponent = pemPublicExponent;
                return para;
            }

            private static RSAParameters ConvertFromPrivateKey(string pemFileConent)
            {
                byte[] keyData = Convert.FromBase64String(pemFileConent);
                if (keyData.Length < 609)
                {
                    throw new ArgumentException("pem file content is incorrect.");
                }

                int index = 11;
                byte[] pemModulus = new byte[128];
                Array.Copy(keyData, index, pemModulus, 0, 128);

                index += 128;
                index += 2;//141
                byte[] pemPublicExponent = new byte[3];
                Array.Copy(keyData, index, pemPublicExponent, 0, 3);

                index += 3;
                index += 4;//148
                byte[] pemPrivateExponent = new byte[128];
                Array.Copy(keyData, index, pemPrivateExponent, 0, 128);

                index += 128;
                index += ((int)keyData[index + 1] == 64 ? 2 : 3);//279
                byte[] pemPrime1 = new byte[64];
                Array.Copy(keyData, index, pemPrime1, 0, 64);

                index += 64;
                index += ((int)keyData[index + 1] == 64 ? 2 : 3);//346
                byte[] pemPrime2 = new byte[64];
                Array.Copy(keyData, index, pemPrime2, 0, 64);

                index += 64;
                index += ((int)keyData[index + 1] == 64 ? 2 : 3);//412/413
                byte[] pemExponent1 = new byte[64];
                Array.Copy(keyData, index, pemExponent1, 0, 64);

                index += 64;
                index += ((int)keyData[index + 1] == 64 ? 2 : 3);//479/480
                byte[] pemExponent2 = new byte[64];
                Array.Copy(keyData, index, pemExponent2, 0, 64);

                index += 64;
                index += ((int)keyData[index + 1] == 64 ? 2 : 3);//545/546
                byte[] pemCoefficient = new byte[64];
                Array.Copy(keyData, index, pemCoefficient, 0, 64);

                RSAParameters para = new RSAParameters();
                para.Modulus = pemModulus;
                para.Exponent = pemPublicExponent;
                para.D = pemPrivateExponent;
                para.P = pemPrime1;
                para.Q = pemPrime2;
                para.DP = pemExponent1;
                para.DQ = pemExponent2;
                para.InverseQ = pemCoefficient;
                return para;
            }
            #endregion

        }
        */


        #region RSA
        /// <summary>
        /// 类名：RSAFromPkcs8
        /// 功能：RSA解密、签名、验签
        /// 详细：该类对Java生成的密钥进行解密和签名以及验签专用类，不需要修改
        /// 版本：2.0
        /// 修改日期：2011-05-10
        /// 说明：
        /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
        /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
        /// </summary>
        internal sealed class RSA
        {
            /// <summary>
            /// 签名
            /// </summary>
            /// <param name="content">需要签名的内容</param>
            /// <param name="privateKey">私钥</param>
            /// <param name="charset">编码格式</param>
            /// <returns></returns>
            public static string Sign(string content, string privateKey, string charset)
            {
                Encoding code = Encoding.GetEncoding(charset);
                byte[] input = code.GetBytes(content);
                using (var rsa = DecodePemPrivateKey(privateKey))
                using (var sha = SHA1.Create())
                {
                    byte[] signData = rsa.SignData(input, sha);
                    return Convert.ToBase64String(signData);
                }

            }
            /// <summary>
            /// 验证签名
            /// </summary>
            /// <param name="content">需要验证的内容</param>
            /// <param name="signedString">签名结果</param>
            /// <param name="publicKey">公钥</param>
            /// <param name="charset">编码格式</param>
            /// <returns></returns>
            public static bool Verify(string content, string signedString, string publicKey, string charset)
            {
                Encoding code = Encoding.GetEncoding(charset);
                byte[] data = Convert.FromBase64String(signedString);

                using (var rsa = new RSACryptoServiceProvider())
                using (var sha = SHA1.Create())
                {
                    rsa.ImportParameters(ConvertFromPublicKey(publicKey));
                    return rsa.VerifyData(code.GetBytes(content), sha, data);
                }

            }

            /// <summary>
            /// 用RSA解密
            /// </summary>
            /// <param name="resData">待解密字符串</param>
            /// <param name="privateKey">私钥</param>
            /// <param name="charset">编码格式</param>
            /// <returns>解密结果</returns>
            public static string DecryptData(string resData, string privateKey, string charset)
            {
                byte[] DataToDecrypt = Convert.FromBase64String(resData);
                List<byte> result = new List<byte>();

                for (int j = 0; j < DataToDecrypt.Length / 128; j++)
                {
                    byte[] buf = new byte[128];
                    for (int i = 0; i < 128; i++)
                    {
                        buf[i] = DataToDecrypt[i + 128 * j];
                    }
                    result.AddRange(Decrypt(buf, privateKey, charset));
                }
                byte[] source = result.ToArray();
                char[] asciiChars = new char[Encoding.GetEncoding(charset).GetCharCount(source, 0, source.Length)];
                Encoding.GetEncoding(charset).GetChars(source, 0, source.Length, asciiChars, 0);
                return new string(asciiChars);
            }

            private static byte[] Decrypt(byte[] data, string privateKey, string input_charset)
            {
                using (var rsa = DecodePemPrivateKey(privateKey))
                using (var sha = SHA1.Create())
                {
                    return rsa.Decrypt(data, false);
                }
            }

            /// <summary>
            /// 解析java生成的pem文件私钥
            /// </summary>
            /// <param name="pemstr"></param>
            /// <returns></returns>
            private static RSACryptoServiceProvider DecodePemPrivateKey(String pemstr)
            {
                byte[] pkcs8privatekey;
                pkcs8privatekey = Convert.FromBase64String(pemstr);
                if (pkcs8privatekey != null)
                {

                    RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8privatekey);
                    return rsa;
                }
                else
                    return null;
            }

            private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
            {

                byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
                byte[] seq = new byte[15];

                MemoryStream mem = new MemoryStream(pkcs8);
                int lenstream = (int)mem.Length;
                BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
                byte bt = 0;
                ushort twobytes = 0;

                try
                {

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;


                    bt = binr.ReadByte();
                    if (bt != 0x02)
                        return null;

                    twobytes = binr.ReadUInt16();

                    if (twobytes != 0x0001)
                        return null;

                    seq = binr.ReadBytes(15);       //read the Sequence OID
                    if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x04) //expect an Octet string 
                        return null;

                    bt = binr.ReadByte();       //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                    if (bt == 0x81)
                        binr.ReadByte();
                    else
                        if (bt == 0x82)
                        binr.ReadUInt16();
                    //------ at this stage, the remaining sequence should be the RSA private key

                    byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                    RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                    return rsacsp;
                }

                catch (Exception)
                {
                    return null;
                }

                finally { binr.Dispose(); }

            }

            private static bool CompareBytearrays(byte[] a, byte[] b)
            {
                if (a.Length != b.Length)
                    return false;
                int i = 0;
                foreach (byte c in a)
                {
                    if (c != b[i])
                        return false;
                    i++;
                }
                return true;
            }

            private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
            {
                byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

                // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
                MemoryStream mem = new MemoryStream(privkey);
                BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
                byte bt = 0;
                ushort twobytes = 0;
                int elems = 0;
                try
                {
                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes != 0x0102) //version number
                        return null;
                    bt = binr.ReadByte();
                    if (bt != 0x00)
                        return null;


                    //------  all private key components are Integer sequences ----
                    elems = GetIntegerSize(binr);
                    MODULUS = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    E = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    D = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    P = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    Q = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    DP = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    DQ = binr.ReadBytes(elems);

                    elems = GetIntegerSize(binr);
                    IQ = binr.ReadBytes(elems);

                    // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                    RSAParameters RSAparams = new RSAParameters();
                    RSAparams.Modulus = MODULUS;
                    RSAparams.Exponent = E;
                    RSAparams.D = D;
                    RSAparams.P = P;
                    RSAparams.Q = Q;
                    RSAparams.DP = DP;
                    RSAparams.DQ = DQ;
                    RSAparams.InverseQ = IQ;
                    RSA.ImportParameters(RSAparams);
                    return RSA;
                }
                catch (Exception)
                {
                    return null;
                }
                finally { binr.Dispose(); }
            }

            private static int GetIntegerSize(BinaryReader binr)
            {
                byte bt = 0;
                byte lowbyte = 0x00;
                byte highbyte = 0x00;
                int count = 0;
                bt = binr.ReadByte();
                if (bt != 0x02)     //expect integer
                    return 0;
                bt = binr.ReadByte();

                if (bt == 0x81)
                    count = binr.ReadByte();    // data size in next byte
                else
                    if (bt == 0x82)
                {
                    highbyte = binr.ReadByte();	// data size in next 2 bytes
                    lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    count = bt;		// we already have the data size
                }



                while (binr.ReadByte() == 0x00)
                {   //remove high order zeros in data
                    count -= 1;
                }
                binr.BaseStream.Seek(-1, SeekOrigin.Current);       //last ReadByte wasn't a removed zero, so back up a byte
                return count;
            }

            #region 解析.net 生成的Pem
            private static RSAParameters ConvertFromPublicKey(string pemFileConent)
            {

                byte[] keyData = Convert.FromBase64String(pemFileConent);
                if (keyData.Length < 162)
                {
                    throw new ArgumentException("pem file content is incorrect.");
                }
                byte[] pemModulus = new byte[128];
                byte[] pemPublicExponent = new byte[3];
                Array.Copy(keyData, 29, pemModulus, 0, 128);
                Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
                RSAParameters para = new RSAParameters();
                para.Modulus = pemModulus;
                para.Exponent = pemPublicExponent;
                return para;
            }

            private static RSAParameters ConvertFromPrivateKey(string pemFileConent)
            {
                byte[] keyData = Convert.FromBase64String(pemFileConent);
                if (keyData.Length < 609)
                {
                    throw new ArgumentException("pem file content is incorrect.");
                }

                int index = 11;
                byte[] pemModulus = new byte[128];
                Array.Copy(keyData, index, pemModulus, 0, 128);

                index += 128;
                index += 2;//141
                byte[] pemPublicExponent = new byte[3];
                Array.Copy(keyData, index, pemPublicExponent, 0, 3);

                index += 3;
                index += 4;//148
                byte[] pemPrivateExponent = new byte[128];
                Array.Copy(keyData, index, pemPrivateExponent, 0, 128);

                index += 128;
                index += ((int)keyData[index + 1] == 64 ? 2 : 3);//279
                byte[] pemPrime1 = new byte[64];
                Array.Copy(keyData, index, pemPrime1, 0, 64);

                index += 64;
                index += ((int)keyData[index + 1] == 64 ? 2 : 3);//346
                byte[] pemPrime2 = new byte[64];
                Array.Copy(keyData, index, pemPrime2, 0, 64);

                index += 64;
                index += ((int)keyData[index + 1] == 64 ? 2 : 3);//412/413
                byte[] pemExponent1 = new byte[64];
                Array.Copy(keyData, index, pemExponent1, 0, 64);

                index += 64;
                index += ((int)keyData[index + 1] == 64 ? 2 : 3);//479/480
                byte[] pemExponent2 = new byte[64];
                Array.Copy(keyData, index, pemExponent2, 0, 64);

                index += 64;
                index += ((int)keyData[index + 1] == 64 ? 2 : 3);//545/546
                byte[] pemCoefficient = new byte[64];
                Array.Copy(keyData, index, pemCoefficient, 0, 64);

                RSAParameters para = new RSAParameters();
                para.Modulus = pemModulus;
                para.Exponent = pemPublicExponent;
                para.D = pemPrivateExponent;
                para.P = pemPrime1;
                para.Q = pemPrime2;
                para.DP = pemExponent1;
                para.DQ = pemExponent2;
                para.InverseQ = pemCoefficient;
                return para;
            }
            #endregion

        }
        #endregion

    }
}