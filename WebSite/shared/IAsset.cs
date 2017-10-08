
namespace Ayatta.Web
{
    public interface IAsset
    {
        IAsset Local(params string[] assets);
        IAsset Static(params string[] assets);
        IAsset Remote(params string[] assets);
        void Refresh();
    }
}