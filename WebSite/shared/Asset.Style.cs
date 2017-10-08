using System;
using System.Linq;
using System.Collections.Generic;

namespace Ayatta.Web
{

    #region Style

    [Flags]
    public enum Css
    {
        FontAwesome = 1,
        Bootstrap = 2,
        BootstrapTheme = 4,
        BootstrapSelect = 8,
        BootstrapDatePicker = 16,
        Fancybox = 32
    }

    #endregion

    #region Style

    public sealed class StyleAsset : AssetBase
    {
        private static readonly IDictionary<Css, string> Dictionary = new Dictionary<Css, string>(10);

        static StyleAsset()
        {
            Dictionary.Add(Css.FontAwesome, "http://cdn.bootcss.com/font-awesome/4.0.3/css/font-awesome.min.css");
            Dictionary.Add(Css.Bootstrap, "//cdn.bootcss.com/bootstrap/3.3.4/css/bootstrap.min.css");
            Dictionary.Add(Css.BootstrapTheme, "http://todc.github.io/todc-bootstrap/dist/css/todc-bootstrap.min.css");
            Dictionary.Add(Css.BootstrapSelect, "//cdn.bootcss.com/select2/4.0.0-rc.2/css/select2.min.css");
        }

        public StyleAsset()
            : base(1)
        {

        }

        public StyleAsset(Css? asset)
            : this()
        {
            foreach (var o in Dictionary.Where(o => (asset & o.Key) == o.Key))
            {
                HashSet.Add(o.Value);
            }
        }
    }

    #endregion

}