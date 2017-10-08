using System;
using System.Linq;
using System.Security.Claims;

namespace Ayatta.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Identity Identity(this ClaimsPrincipal principal)
        {

            if (principal != null && principal.Identity.IsAuthenticated)
            {
                var id = 0;
                string guid = null;

                var temp = principal.Claims.FirstOrDefault(x => x.Type == "guid");
                if (temp != null)
                {
                    guid = temp.Value;
                }
                temp = principal.Claims.FirstOrDefault(x => x.Type == "id");
                if (temp != null)
                {
                    id = Convert.ToInt32(temp.Value);
                }
                var name = principal.Identity.Name;

                return new Identity(id, guid, name);
            }
            return new Identity(0);
        }
    }
}
