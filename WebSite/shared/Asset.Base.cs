using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Ayatta.Web
{
    public abstract class AssetBase : IAsset
    {
        private readonly string prefix;
        private readonly string format;
        private static string version = DateTime.Now.ToString("yyMMddHHmm");
        protected readonly HashSet<string> HashSet = new HashSet<string>();

        protected AssetBase(int category)
        {
            if (category == 1)
            {
                prefix = "/css/";
                format = "<link type=\"text/css\" href=\"{0}\" rel=\"stylesheet\" />";
            }
            if (category == 2)
            {
                prefix = "/js/";
                format = "<script type=\"text/javascript\" src=\"{0}\" ></script>";
            }
        }

        public IAsset Local(params string[] assets)
        {
            foreach (var o in assets.Select(o => prefix + o))
            {
                HashSet.Add(o);
            }
            return this;
        }

        public IAsset Static(params string[] assets)
        {
            foreach (var o in assets.Select(o => WebSite.Static + prefix + o))
            {
                HashSet.Add(o);
            }
            return this;
        }

        public IAsset Remote(params string[] assets)
        {
            foreach (var o in assets)
            {
                HashSet.Add(o);
            }
            return this;
        }

        public void Refresh()
        {
            version = DateTime.Now.ToString("yyMMddHHmm");
        }

        public override string ToString()
        {
            var i = 1;
            var l = HashSet.Count;
            var sb = new StringBuilder();

            foreach (var src in HashSet.Select(o => (o.IndexOf('?') >= 0 ? o + "&v=" : o + "?v=") + version))
            {
                sb.AppendFormat(i == 1 ? format : "    " + format, src);
                if (i < l)
                {
                    sb.AppendLine();
                }
                i++;
            }
            return sb.ToString();

        }
    }
}