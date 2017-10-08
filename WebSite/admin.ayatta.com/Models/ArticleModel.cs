using Ayatta.Domain;

namespace Ayatta.Web.Models
{

    #region 帮助
    public class ArticleListModel : Model
    {
        public string Keyword { get; set; }
        public IPagedList<Article> Articles { get; set; }
    }

    public class ArticleDetailModel : Model
    {
        public Article Article { get; set; }
    }
    #endregion
}
