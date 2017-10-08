using System;
using System.Linq;
using System.Collections.Generic;

namespace Ayatta.Web
{
    #region

    [Flags]
    public enum Js
    {
        Lodash = 1,

        JQuery = 2,
        JQueryExtend = 4,
        JQueryValidate = 8,
        JQueryValidateExtend = 16,

        Bootstrap = 32,
        BootstrapExtend = 64,
        BootstrapSelect = 128,
        BootstrapDatePicker = 256,

        JQuerySlide = 512,
        JQueryMenuAim = 1024,
        JQueryElevateZoom = 2048,

        Fancybox = 4096
    }

    #endregion

    #region ScriptAsset

    public sealed class ScriptAsset : AssetBase
    {
        private static readonly IDictionary<Js, string> Dictionary = new Dictionary<Js, string>(10);

        static ScriptAsset()
        {
            Dictionary.Add(Js.Lodash, "http://cdn.bootcss.com/underscore.js/1.6.0/underscore-min.js");
            Dictionary.Add(Js.JQuery, "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.11.0.min.js");
            Dictionary.Add(Js.JQueryExtend, WebSite.Static + "/js/jquery.extend.js");
            Dictionary.Add(Js.JQueryValidate, "http://ajax.aspnetcdn.com/ajax/jquery.validate/1.11.1/jquery.validate.min.js");
            Dictionary.Add(Js.JQueryValidateExtend, WebSite.Static + "/js/jquery.validate.extend.js");
            Dictionary.Add(Js.JQuerySlide, WebSite.Static + "/js/jquery.slides.min.js");
            Dictionary.Add(Js.JQueryElevateZoom, WebSite.Static + "/js/jquery.elevatezoom.js");

            Dictionary.Add(Js.Bootstrap, "http://netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js");
            Dictionary.Add(Js.BootstrapExtend, WebSite.Static + "/js/bootstrap.extend.js");
            Dictionary.Add(Js.BootstrapSelect, "http://todc.github.io/todc-select2/select2-3.2/select2.js");
            Dictionary.Add(Js.BootstrapDatePicker, "http://todc.github.io/todc-datepicker/assets/js/bootstrap-datepicker.js");
        }

        public ScriptAsset()
            : base(2)
        {

        }

        public ScriptAsset(Js? asset)
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