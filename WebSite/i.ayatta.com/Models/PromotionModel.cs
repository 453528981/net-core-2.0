using Ayatta;
using Ayatta.Domain;
using Ayatta.Web.Models;

namespace Ayatta.Web
{
    public static class PromotionModel
    {
        public class ActivityList : Model
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string StartedOn { get; set; }

            public string StoppedOn { get; set; }

            public bool Global { get; set; }
            public IPagedList<Promotion.Activity> Promotions { get; set; }
        }

        public class ActivityDetail : Model
        {
            public bool Global { get; set; }
            public Promotion.Activity Promotion { get; set; }
            /*
            public Rule[] Rules { get; set; }
            public class Rule
            {
                public int Index { get; set; }
                public int Upon { get; set; }
                public int Value { get; set; }

                public bool ValueSelected { get; set; }
                public bool FreightFreeSelected { get; set; }
                public string FreightFreeExclude { get; set; }
                public string CouponData { get; set; }
                public bool CouponSelected { get; set; }
                public string GiftData { get; set; }
                public bool GiftSelected { get; set; }
            }
            */
        }

        public class Packages : Model
        {
            public IPagedList<Promotion.Package> Promotions { get; set; }
        }
    }
}