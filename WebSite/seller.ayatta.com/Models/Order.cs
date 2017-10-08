using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ayatta.Domain;

namespace Ayatta.Web.Models
{
    public class OrderListModel : Model
    {
        public IPagedList<Order> Orders { get; set; }
    }

    public class OrderDetailModel : Model
    {
        public Order Order{ get; set; }
    }
}
