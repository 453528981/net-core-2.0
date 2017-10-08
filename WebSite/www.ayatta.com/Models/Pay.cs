using Ayatta.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayatta.Web.Models
{
    public class PayOrder : Model
    {

        public Account Account { get; set; }
        public Order Order { get; set; }
        public IList<PaymentBank> Banks { get; set; }
        public IList<PaymentPlatform> Platforms { get; set; }

    }
}
