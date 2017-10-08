using Ayatta;
using Ayatta.Cart;
using Ayatta.Domain;
using System.Collections.Generic;

using System.Linq;

namespace Ayatta.Web.Models
{
    public static class CartModel
    {
        public class Index : Model
        {
            public IList<string> Data { get; set; }

            public CartData CartData { get; set; }

        }

        public class Confirm : Model
        {
            public Status Status { get; set; }
            public CartData CartData { get; set; }
            public IList<UserAddress> Addresses { get; set; }

            /// <summary>
            /// 是否支持货到付款
            /// </summary>
            public bool SupportCOD { get; set; }

            public int AddressId
            {
                get
                {
                    if (CartData != null && CartData.UserAddress != null)
                    {
                        return CartData.UserAddress.Id;
                    }
                    return 0;
                }
            }


        }
    }
}