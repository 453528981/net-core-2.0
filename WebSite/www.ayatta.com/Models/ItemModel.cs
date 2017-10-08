using Ayatta.Domain;

namespace Ayatta.Web.Models
{
    public static class ItemModel
    {
        public class Index : Model
        {
            public Item Item { get; set; }
            public int SkuId { get; set; }
            public string[] SelectdProps { get; set; }

        }
    }
}