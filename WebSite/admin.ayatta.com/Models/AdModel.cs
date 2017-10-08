using Ayatta;
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Web.Models
{
    #region 


    public class AdItemListModel : Model
    {
        public AdModule Module { get; set; }
        public int ModuleId { get; set; }
        public string Keyword { get; set; }
        public IPagedList<AdItem> Items { get; set; }
    }

    public class AdItemDetailModel : Model
    {
        public AdItem Item { get; set; }
    }

    #endregion


}