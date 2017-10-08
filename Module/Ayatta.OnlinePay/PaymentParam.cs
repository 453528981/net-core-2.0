using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Primitives;

namespace Ayatta.OnlinePay
{
    public class PaymentParam : SortedDictionary<string, string>
    {
        public PaymentParam() : this(new List<KeyValuePair<string, StringValues>>())
        {

        }
        public PaymentParam(IEnumerable<KeyValuePair<string, StringValues>> kvs)
        {
            foreach (var kv in kvs)
            {
                Add(kv.Key, kv.Value.ToString().Trim());
            }
        }
        public PaymentParam(Dictionary<string, string> dic)
        {
            foreach (var kv in dic)
            {
                Add(kv.Key, kv.Value);
            }
        }
        public PaymentParam(PaymentParam dic)
        {
            foreach (var kv in dic)
            {
                Add(kv.Key, kv.Value);
            }
        }

        public PaymentParam(XContainer xml)
        {
            var nodes = xml.Descendants();

            foreach (var node in nodes)
            {
                Add(node.Name.LocalName, node.Value);
            }
        }

        public new PaymentParam Add(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return this;

            var k = key.Trim();
            this[k] = value.Trim();
            return this;
        }

        public PaymentParam Remove(params string[] keys)
        {
            foreach (var key in keys.Where(ContainsKey))
            {
                base.Remove(key);
            }
            return this;
        }

        public int GetInt(string key, int defaultVal = 0)
        {
            if (!ContainsKey(key)) return defaultVal;

            var val = this[key];
            return Convert.ToInt32(val);
        }

        public decimal GetDecimal(string key, decimal defaultVal = 0)
        {
            if (!ContainsKey(key)) return defaultVal;

            var val = this[key];
            return Convert.ToDecimal(val);
        }

        public string GetString(string key, string defaultVal = "")
        {
            if (!ContainsKey(key)) return defaultVal;

            var val = this[key];
            return val;
        }
        public bool GetBool(string key, bool defaultVal = false)
        {
            if (!ContainsKey(key)) return defaultVal;

            var val = this[key];
            return Convert.ToBoolean(val);
        }

        public DateTime GetDateTime(string key, DateTime defaultVal)
        {
            if (!ContainsKey(key)) return defaultVal;

            var val = this[key];
            return Convert.ToDateTime(val);
        }

        public string ToQueryString(bool includeQuestionMark = false, bool skipEmpty = false, bool urlEncode = false)
        {
            if (Keys.Count < 1) return string.Empty;
            var qs = new StringBuilder();
            if (includeQuestionMark)
            {
                qs.Append("?");
            }
            var i = 0;
            foreach (var key in Keys)
            {
                if (i > 0)
                {
                    qs.Append("&");
                }
                var value = this[key];
                if (string.IsNullOrEmpty(value))
                {
                    if (urlEncode)
                    {
                        qs.AppendFormat("{0}={1}", UrlEncoder.Default.Encode(key.Trim()), UrlEncoder.Default.Encode(value));
                    }
                    else
                    {
                        qs.AppendFormat("{0}={1}", key.Trim(), value);
                    }
                }
                else
                {
                    if (urlEncode)
                    {
                        qs.AppendFormat("{0}={1}", UrlEncoder.Default.Encode(key.Trim()), UrlEncoder.Default.Encode(value));
                    }
                    else
                    {
                        qs.AppendFormat("{0}={1}", key.Trim(), value);
                    }
                }
                i++;
            }
            return qs.ToString();
        }

        public string ToXml()
        {
            var regex = new Regex(@"^[0-9.]$");

            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (var key in Keys)
            {
                var value = this[key];
                if (regex.IsMatch(value))
                {
                    sb.Append("<" + key + ">" + value + "</" + key + ">");
                }
                else
                {
                    sb.Append("<" + key + "><![CDATA[" + value + "]]></" + key + ">");
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        public string ToJson()
        {
            var i = 1;
            var len = Keys.Count;
            var regex = new Regex(@"^[0-9.]$");

            var sb = new StringBuilder();
            sb.Append("{");
            foreach (var key in Keys)
            {
                var value = this[key];
                if (regex.IsMatch(value) || value.ToLower() == "true" || value.ToLower() == "false")
                {
                    sb.AppendFormat("\"{0}\":{1}", key, value);
                }
                else
                {
                    sb.AppendFormat("\"{0}\":\"{1}\"", key, HtmlEncoder.Default.Encode(value));
                }
                if (i < len)
                {
                    sb.Append(",");
                }
                i++;
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}