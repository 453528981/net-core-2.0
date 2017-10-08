using System;
using Ayatta.Api;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;

namespace Ayatta.Web.Controllers
{
    /// <summary>
    /// 其它接口
    /// </summary>
    public class MiscController : BaseController
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public MiscController(IHttpContextAccessor httpContextAccessor, DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<MiscController> logger) : base(defaultStorage, defaultCache, logger)
        {
            this.httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("file-upload")]
        public FileUploadResponse FileUpload(FileUploadRequest req)
        {
            var rep = new FileUploadResponse();
            var context = httpContextAccessor.HttpContext;
            var files = context.Request.Form.Files;

            return rep;
        }

        /// <summary>
        /// 行政区列表
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("region-list")]
        public RegionListResponse RegionList([FromBody]RegionListRequest req)
        {
            var rep = new RegionListResponse();

            var list = RegionList();

            if (req.Hierarchical)
            {

                rep.China = list.ToChina();
            }
            else
            {
                if (string.IsNullOrEmpty(req.ParentId))
                {
                    rep.Data = list;
                }
                else
                {
                    rep.Data = list.Where(x => x.ParentId == req.ParentId).ToList();
                }
            }

            return rep;
        }

    }
}