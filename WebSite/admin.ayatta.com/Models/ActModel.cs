using Ayatta;
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Web.Models
{
    #region ¹Ù·½»î¶¯
    public class ActPlanListModel : Model
    {
        public string Keyword { get; set; }
        public IPagedList<ActPlan> Plans { get; set; }
    }

    public class ActPlanDetailModel : Model
    {
        public ActPlan Plan { get; set; }
    }

    public class ActItemListModel : Model
    {
        public string PlanId { get; set; }
        public string Keyword { get; set; }
        public IPagedList<ActItem> Items { get; set; }
    }

    public class ActItemDetailModel : Model
    {
        public ActItem Item { get; set; }
    }

    #endregion


}