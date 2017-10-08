using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Ayatta.Web
{
    #region Js/Css资源管理
    public static class Asset
    {
        public static IAsset Script()
        {
            return Script(null);
        }

        public static IAsset Script(Js? asset)
        {
            return new ScriptAsset(asset);
        }

        public static IAsset Style()
        {
            return Style(null);
        }

        public static IAsset Style(Css? asset)
        {
            return new StyleAsset(asset);
        }
    }
    #endregion
}