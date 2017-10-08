using System;
using System.Linq;
using Ayatta.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

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
