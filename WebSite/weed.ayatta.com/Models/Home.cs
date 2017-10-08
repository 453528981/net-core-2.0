using Ayatta.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayatta.Web.Models
{
    public class ExploreModel : Model
    {
        public IDictionary<int, string> Dirs { get; set; }
        //public IPagedList<WeedFile> Files { get; set; }

        public IList<Node> Nodes { get; set; }
    }
}
