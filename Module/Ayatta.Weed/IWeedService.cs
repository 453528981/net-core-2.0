using System;
using System.IO;
using System.Threading.Tasks;

namespace Ayatta.Weed
{
    public interface IWeedService
    {
        Action<UploadResult> OnUpload { get; set; }
        Task<UploadResult> Upload(string name, Stream stream, int uid = 0, int did = 0);
    }
}
