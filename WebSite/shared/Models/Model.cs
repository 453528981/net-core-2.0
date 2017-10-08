namespace Ayatta.Web.Models
{
    public interface IModel
    {

    }
    public abstract class Model : IModel
    {
        private const string TitleSuffix = " - Tiantian";

        private string title;

        public string Title { get { return title + TitleSuffix; } set { title = value; } }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string SelectedMenu { get; set; }

    }

    public class Model<T> : Model
    {
        public T Data { get; set; }

        public Model() : this(default(T))
        {

        }
        public Model(T data)
        {
            Data = data;
        }
    }


    public class Model<T, TExtra> : Model<T>
    {
        public TExtra Extra { get; set; }

        public Model(T data):this(data,default(TExtra))
        {
           
        }
        public Model(T data, TExtra extra)
        {
            Data = data;
            Extra = extra;
        }
    }

}
