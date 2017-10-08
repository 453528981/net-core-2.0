using System;
using System.Collections.Generic;
using System.Text;

namespace Ayatta.Storage.Param
{
    public class PagedParam
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
