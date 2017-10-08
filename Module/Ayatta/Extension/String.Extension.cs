using System;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Ayatta.Extension
{
    public static partial class Extension
    {
        private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex StripHtmlExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 去除所有标签，只允许img,p,strong标签
        /// </summary>
        private static readonly Regex RemoveHtmlExpression = new Regex("<(?!img|p|/p|strong|/strong).*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);


        private static readonly char[] IllegalUrlCharacters = new[]
            {
                ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',', '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(',
                ')', '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘', '’', '“', '”', '»', '«'
            };

        public static string Hash(this string input)
        {
            using (var h = MD5.Create())
            {
                var bs = h.ComputeHash(Encoding.UTF8.GetBytes(input));
                return bs.ToHex();
            }
        }

        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }

        public static string IfNullEmpty(this string target)
        {
            return target ?? string.Empty;
        }

        public static bool IsMatch(this string target, string pattern)
        {
            return target != null && Regex.IsMatch(target, pattern);
        }

        public static string Match(this string target, string pattern)
        {
            return target == null ? string.Empty : Regex.Match(target, pattern).Value;
        }

        /// <summary>
        /// 是否为密码字符串
        /// 要求密码长度为6-16位，包含至少1个特殊字符，2个数字，2个字母。 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsPassword(this string target)
        {
            const string reg = @"^(?=^.{6,12}$)(?=(?:.*?\d){2})(?=.*[a-z])(?=(?:.*?[a-z]){2})(?=(?:.*?[!@#$%*()_+^&}{:;?.]){1})(?!.*\s)[0-9a-zA-Z!@#$%*()_+^&]*$";
            return !string.IsNullOrEmpty(target) && IsMatch(target, reg);
        }

        /// <summary>
        /// 是否为密码字符串
        /// 要求密码长度为6-18位，包含至少1个特殊字符，2个数字，2个大写字母和一些小写字母。 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsStrongPassword(this string target)
        {
            const string reg = @"^(?=^.{6,18}$)(?=(?:.*?\d){2})(?=.*[a-z])(?=(?:.*?[A-Z]){2})(?=(?:.*?[!@#$%*()_+^&}{:;?.]){1})(?!.*\s)[0-9a-zA-Z!@#$%*()_+^&]*$";
            return !string.IsNullOrEmpty(target) && IsMatch(target, reg);
        }

        public static bool IsMobile(this string target)
        {
            const string reg = @"^(13\d{9}$)|(15[0135-9]\d{8}$)|(18[2679]\d{8})$";
            return !string.IsNullOrEmpty(target) && IsMatch(target, reg);
        }

        public static bool IsPhone(this string target)
        {
            const string reg = @"^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$";
            return !string.IsNullOrEmpty(target) && IsMatch(target, reg);
        }

        public static bool IsPostalCode(this string target)
        {
            const string reg = @"^\d{6}?$";
            return !string.IsNullOrEmpty(target) && IsMatch(target, reg);
        }

        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }

        public static bool IsEmail(this string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        public static string FormatWith(this string target, params object[] args)
        {
            return string.Format(target, args);
        }

        public static string Reverse(this string target)
        {
            var input = target.ToCharArray();
            var output = new char[target.Length];
            for (var i = 0; i < input.Length; i++)
                output[input.Length - 1 - i] = input[i];
            return new string(output);
        }

        public static string WrapAt(this string target, int index)
        {
            const int dotCount = 3;
            return (target.Length <= index)
                       ? target
                       : string.Concat(target.Substring(0, index - dotCount), new string('.', dotCount));
        }

        public static string StripHtml(this string target)
        {
            return StripHtmlExpression.Replace(target, string.Empty);
        }

        public static string RemoveHtml(this string target)
        {
            return RemoveHtmlExpression.Replace(target, string.Empty);
        }

        public static string ToBase64String(this string target)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(target));
        }

        public static string ToBase64String(this string target, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(target));
        }

        public static string ToCamel(this string target)
        {
            if (target.IsNullOrEmpty()) return target;
            return target[0].ToString().ToLower() + target.Substring(1);
        }

        public static string ToPascal(this string target)
        {
            if (target.IsNullOrEmpty()) return target;
            return target[0].ToString().ToUpper() + target.Substring(1);
        }

        //[DebuggerStepThrough]
        //public static Guid ToGuid(this string target)
        //{
        //    var result = Guid.Empty;

        //    if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
        //    {
        //        var encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

        //        try
        //        {
        //            var base64 = Convert.FromBase64String(encoded);

        //            result = new Guid(base64);
        //        }
        //        catch (FormatException)
        //        {
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// 转全角(SBC case)
        /// </summary>
        /// <param name="target">任意字符串</param>
        /// <returns>全角字符串</returns>
        public static string ToSbc(this string target)
        {
            var c = target.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 转半角(DBC case)
        /// </summary>
        /// <param name="target">任意字符串</param>
        /// <returns>半角字符串</returns>
        public static string ToDbc(this string target)
        {
            var c = target.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            var convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        public static string Replace(this string target, ICollection<string> oldValues, string newValue)
        {
            oldValues.ForEach(oldValue => target = target.Replace(oldValue, newValue));
            return target;
        }

        ///   <summary>   
        ///   截取字符串   
        ///   </summary>   
        ///   <param   name="strInput">传入字符串</param>   
        ///   <param   name="strEnd">发生截取后的后缀（例如：...）</param>   
        ///   <param   name="intLen">截取后的长度（包括后缀，全角占两位）</param>   
        ///   <returns>截取好的字符串</returns>   
        public static string ToCutString(this string strInput, string strEnd, int intLen)
        {
            strInput = strInput.Trim();
            var byteLen = Encoding.UTF8.GetByteCount(strInput);
            if (byteLen <= intLen)
            {
                return strInput;
            }
            var resultStr = string.Empty;
            for (var i = 0; i < strInput.Length; i++)
            {
                if (Encoding.UTF8.GetByteCount(resultStr) < intLen - strEnd.Length)
                {
                    resultStr += strInput.Substring(i, 1);
                }
                else
                {
                    break;
                }
            }
            return resultStr + strEnd;
        }

        /// <summary>
        /// 如果第一个字符为英文直接返回英文首字母并转换为大写
        /// 如果第一个字符为中文则返回第一个字符的中文拼音的首字母并转换为大写
        /// </summary>
        /// <param name="target">字符</param>
        /// <returns>大写拼音首字母</returns>
        public static string ToCnInitia(this string target)
        {
            var cnChar = target.Substring(0, 1);
            var arrCn = Encoding.UTF8.GetBytes(cnChar);
            if (arrCn.Length > 1)
            {
                int area = arrCn[0];

                int pos = arrCn[1];
                var code = (area << 8) + pos;
                int[] areacode =
                    {
                        45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324,
                        49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481
                    };
                for (var i = 0; i < 26; i++)
                {
                    var max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.UTF8.GetString(new[] { (byte)(65 + i) });
                    }
                }
                return string.Empty;
            }
            return cnChar;
        }

        /*
        /// <summary>
        /// Aes加密 返回HexString
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesEncrypt(this string target, string key)
        {
            var keyArray = Encoding.UTF8.GetBytes(key);
            var toEncryptArray = Encoding.UTF8.GetBytes(target);
            var rDel = new RijndaelManaged { Key = keyArray, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
            var cTransform = rDel.CreateEncryptor();
            var bytes = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return bytes.ToHex();
        }

        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="target">HexString</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecrypt(this string target, string key) //"2rV92fe_e3.tu5A8"
        {
            var keyArray = Encoding.UTF8.GetBytes(key);
            var toEncryptArray = HexToByte(target);
            var rijndaelManaged = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var cryptoTransform = rijndaelManaged.CreateDecryptor();
            var resultArray = cryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
        */
        /// <summary>
        /// string转16进制byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexToByte(this string hexString)
        {
            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 16进制byte[]转string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHex(this IEnumerable<byte> bytes)
        {
            var hex = new StringBuilder();
            foreach (var b in bytes)
            {
                hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
        }

        public static string ToString(this string[] array, char join)
        {
            if (array.Length == 1)
            {
                return array[0];
            }
            if (array.Length > 1)
            {
                var sb = new StringBuilder(array.Length);
                foreach (var s in array)
                {
                    sb.Append(s + join);
                }
                return sb.ToString().TrimEnd(join);
            }
            return string.Empty;
        }

        /// <summary>检查字符串值是否为 null 或空。</summary>
        /// <returns>如果 <paramref name="value" /> 为 null 或零长度字符串 ("")，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>将字符串转换为整数。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        public static int AsInt(this string value)
        {
            return value.AsInt(0);
        }

        /// <summary>将字符串转换为整数，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 或无效的值时要返回的值。</param>
        public static int AsInt(this string value, int defaultValue)
        {
            int result;
            return !int.TryParse(value, out result) ? defaultValue : result;
        }

        /// <summary>将字符串转换为 <see cref="T:System.Decimal" /> 数字。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        public static decimal AsDecimal(this string value)
        {
            return value.AsDecimal(0);
        }

        /// <summary>将字符串转换为 <see cref="T:System.Decimal" /> 数字，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 或无效时要返回的值。</param>
        public static decimal AsDecimal(this string value, decimal defaultValue)
        {
            decimal result;
            return !decimal.TryParse(value, out result) ? defaultValue : result;
        }

        /// <summary>将字符串转换为 <see cref="T:System.Single" /> 数字。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        public static float AsFloat(this string value)
        {
            return value.AsFloat(0f);
        }

        /// <summary>将字符串转换为 <see cref="T:System.Single" /> 数字，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 时要返回的值。</param>
        public static float AsFloat(this string value, float defaultValue)
        {
            float result;
            return !float.TryParse(value, out result) ? defaultValue : result;
        }

        /// <summary>将字符串转换为 <see cref="T:System.DateTime" /> 值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        public static DateTime AsDateTime(this string value)
        {
            return value.AsDateTime(default(DateTime));
        }

        /// <summary>将字符串转换为 <see cref="T:System.DateTime" /> 值，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 或无效的值时要返回的值。默认值为系统的最小时间值。</param>
        public static DateTime AsDateTime(this string value, DateTime defaultValue)
        {
            DateTime result;
            return !DateTime.TryParse(value, out result) ? defaultValue : result;
        }

        /// <summary>将字符串转换为布尔值 (true/false)。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        public static bool AsBool(this string value)
        {
            return value.AsBool(false);
        }

        /// <summary>将字符串转换为布尔值 (true/false)，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 或无效的值时要返回的值。</param>
        public static bool AsBool(this string value, bool defaultValue)
        {
            bool result;
            return !bool.TryParse(value, out result) ? defaultValue : result;
        }

        /// <summary>检查字符串是否可以转换为 Boolean (true/false) 类型。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsBool(this string value)
        {
            bool flag;
            return bool.TryParse(value, out flag);
        }

        /// <summary>检查字符串是否可以转换为整数。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsInt(this string value)
        {
            int num;
            return int.TryParse(value, out num);
        }

        /// <summary>检查字符串是否可以转换为 <see cref="T:System.Decimal" /> 类型。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsDecimal(this string value)
        {
            decimal num;
            return decimal.TryParse(value, out num);
        }

        /// <summary>检查字符串是否可以转换为 <see cref="T:System.Single" /> 类型。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsFloat(this string value)
        {
            float num;
            return float.TryParse(value, out num);
        }

        /// <summary>检查字符串是否可以转换为 <see cref="T:System.DateTime" /> 类型。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的字符串值。</param>
        public static bool IsDateTime(this string value)
        {
            DateTime dateTime;
            return DateTime.TryParse(value, out dateTime);
        }

        /*
         * 
        /// <summary>将字符串转换为指定数据类型的强类型值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <typeparam name="TValue">要转换为的数据类型。</typeparam>
        public static TValue As<TValue>(this string value)
        {
            return value.As(default(TValue));
        }

        /// <summary>将字符串转换为指定的数据类型，并指定默认值。</summary>
        /// <returns>转换后的值。</returns>
        /// <param name="value">要转换的值。</param>
        /// <param name="defaultValue">当 <paramref name="value" /> 为 null 时要返回的值。</param>
        /// <typeparam name="TValue">要转换为的数据类型。</typeparam>
        public static TValue As<TValue>(this string value, TValue defaultValue)
        {
            try
            {
                var converter =TypeDescriptor.GetConverter(typeof(TValue));
                if (converter.CanConvertFrom(typeof(string)))
                {
                    var result = (TValue)(converter.ConvertFrom(value));
                    return result;
                }
                converter = TypeDescriptor.GetConverter(typeof(string));
                if (converter.CanConvertTo(typeof(TValue)))
                {
                    var result = (TValue)(converter.ConvertTo(value, typeof(TValue)));
                    return result;
                }
            }
            catch
            {
                return defaultValue;
            }
            return defaultValue;
        }

        /// <summary>检查字符串是否可以转换为指定的数据类型。</summary>
        /// <returns>如果 <paramref name="value" /> 可以转换为指定的类型，则为 true；否则为 false。</returns>
        /// <param name="value">要测试的值。</param>
        /// <typeparam name="TValue">要转换为的数据类型。</typeparam>
        public static bool Is<TValue>(this string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(TValue));
            try
            {
                if (value == null || converter.CanConvertFrom(null, value.GetType()))
                {
                    converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        */
    }
}