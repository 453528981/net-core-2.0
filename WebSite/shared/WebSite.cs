
namespace Ayatta.Web
{
    public static class WebSite
    {
        public static readonly string CookieDomain = "ikeepme.com";


        private static readonly string tpl = "http://{0}.{1}";
        private static readonly string MainDomain = "ikeepme.com:8080";
        public static string Main { get { return string.Format(tpl, "www", MainDomain); } }
        public static string Cart { get { return string.Format(tpl, "cart", MainDomain); } }
        public static string Item { get { return string.Format(tpl, "item", MainDomain); } }

        public static string Asset { get { return string.Format(tpl, "asset", MainDomain); } }

        public static string Static { get { return string.Format(tpl, "static", MainDomain); } }
        public static string Game { get { return string.Format(tpl, "game", MainDomain); } }
        public static string Passport { get { return string.Format(tpl, "passport", MainDomain); } }
        public static string Pay { get { return string.Format(tpl, "pay", MainDomain); } }
        public static string My { get { return string.Format(tpl, "my", MainDomain); } }

        public static string Seller { get { return string.Format(tpl, "seller", MainDomain); } }
        public static string Logistics { get { return string.Format(tpl, "logistics", MainDomain); } }

        //public static string GenerateCartOperationUrl(int itemId, int skuId, int quantity = 0, int max = int.MaxValue)
        //{
        //    if (quantity > 0)
        //    {
        //        if (quantity > max)
        //        {
        //            return string.Format("{0}/cart?itemId={1}&skuId={2}&quantity={3}", Cart, itemId, skuId, quantity - 1);
        //        }
        //        return string.Format("{0}/cart?itemId={1}&skuId={2}&quantity={3}", Cart, itemId, skuId, quantity);
        //    }
        //    return string.Format("{0}/cart?itemId={1}&skuId={2}&opt=1", Cart, itemId, skuId);
        //}
    }
}