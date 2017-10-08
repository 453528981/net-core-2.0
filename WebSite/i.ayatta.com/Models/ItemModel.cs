using Ayatta;
using Ayatta.Domain;
using Ayatta.Web.Models;

namespace Ayatta.Web
{
    public static class ItemModel
    {
        public class Index : Model
        {
            //public ProdItemSearchParam Param { get; set; }
            public IPagedList<Ayatta.Domain.Item> Items { get; set; }
        }

        public class Catg : Model
        {
            public string Name { get; set; }
            //public IList<Data.Catg> Catgs { get; set; }

        }

        public class Item : Model<ItemMini>
        {
            public int CatgId { get; set; }

            public ItemDesc ItemDesc { get; set; }
            public Item(ItemMini data) : base(data)
            {
            }


        }

    }
}