using Ayatta.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayatta.Web.Models
{
    public class AddressListModel : Model
    {
        public IList<UserAddress> Addresses { get; set; }
    }

    public class AddressDetailModel : Model
    {
        public UserAddress Address { get; set; }

        public IList<Region> Provinces { get; set; }

        public IList<Region> Citys { get; set; }

        public IList<Region> Districts { get; set; }
    }
}
