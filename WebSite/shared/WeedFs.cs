using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Ayatta.Web
{
    public sealed class WeedFs
    {
        private readonly HttpClient hc;

        public static readonly WeedFs Instance = new WeedFs();

        private WeedFs()
        {
            hc = new HttpClient();
            
            hc.BaseAddress = new Uri("http://localhost:8888/");
            hc.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<Weed> Explore(string dir = null, string lastFileName = null)
        {
            if (!string.IsNullOrEmpty(dir))
            {
                if (!dir.StartsWith("/"))
                {
                    dir = "/" + dir;
                }
                if (!dir.EndsWith("/"))
                {
                    dir += "/";
                }
            }
            var json = await hc.GetStringAsync(dir);           
            return JsonConvert.DeserializeObject<Weed>(json);
        }

        public async Task<Result<string>> Upload(string path, Stream stream)
        {
            var result = new Result<string>();
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream));
            var hrm = await hc.PostAsync(path, content);
            var json = await hrm.Content.ReadAsStringAsync();
            if (!json.Contains("error"))
            {
                result.Status = true;
                var o = JObject.Parse(json);
                result.Data = o.Value<string>("fid");
                result.Message = o.Value<string>("name");
            }
            return result;
        }
    }
}
