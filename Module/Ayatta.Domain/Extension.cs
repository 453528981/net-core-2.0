using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    public  static partial class Extension
    {
        public static bool IsOnlinePay(this PaymentType type)
        {
            return (type == PaymentType.Alipay || type == PaymentType.Weixin || type == PaymentType.Tenpay);
        }
        public static China ToChina(this IList<Region> regions)
        {
            var china = new China();
            china.Provinces = new List<China.Province>();

            var ps = regions.Where(x => x.ParentId == "86");
            foreach (var p in ps)
            {
                var pv = new China.Province();
                pv.Id = p.Id;
                pv.Name = p.Name;
                pv.PostalCode = p.PostalCode;

                pv.Cities = new List<China.Province.City>();

                var cs = regions.Where(x => x.ParentId == p.Id);
                foreach (var c in cs)
                {
                    var cv = new China.Province.City();
                    cv.Id = c.Id;
                    cv.Name = c.Name;
                    cv.PostalCode = c.PostalCode;

                    cv.Districts = new List<China.Province.City.District>();

                    var ds = regions.Where(x => x.ParentId == c.Id);
                    foreach (var d in ds)
                    {
                        var dv = new China.Province.City.District();
                        dv.Id = d.Id;
                        dv.Name = d.Name;
                        dv.PostalCode = d.PostalCode;

                        cv.Districts.Add(dv);
                    }
                    pv.Cities.Add(cv);

                }
                china.Provinces.Add(pv);
            }
            return china;
        }
    }

}
