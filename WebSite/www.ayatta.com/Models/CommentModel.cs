using Ayatta.Domain;

namespace Ayatta.Web.Models
{
    public class CommentIndexModel : Model
    {
        public ItemMini Item { get; set; }
        public ItemComment Comment { get; set; }
        public IPagedList<Comment> Comments { get; set; }

    }

}