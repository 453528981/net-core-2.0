using System;
using System.Linq;
using Ayatta.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Ayatta.Web.Models
{
    public class SecurityIndexModel : Model
    {
       public User User { get; set; }
    }
}
