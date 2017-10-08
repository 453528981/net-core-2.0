using System;
using System.IO;
using System.Linq;
using Ayatta.Weed;
using Ayatta.Domain;
using Ayatta.Storage;
using Newtonsoft.Json;
using Ayatta.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IWeedService weedService;
        public HomeController(IWeedService weedService, DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<HomeController> logger) : base(defaultStorage, defaultCache, logger)
        {
            this.weedService = weedService;
            if (this.weedService.OnUpload == null)
            {
                this.weedService.OnUpload += OnUpload;
            }
        }

        [HttpGet("explore/{pid?}")]
        public IActionResult Explore(int pid)
        {
            var did = 0;
            var now = DateTime.Now;
            var identity = User;

            var model = new ExploreModel();
            var data = DefaultStorage.WeedDirList(identity.Id);
            if (data.Count == 0)
            {
                var o = new WeedDir();
                o.Pid = 0;
                o.Name = "目录";
                o.Depth = 1;
                o.UserId = 1;
                o.Status = true;
                //o.CreatedBy = o.CreatedBy;
                o.CreatedOn = now;
                //o.ModifiedBy = o.ModifiedBy;
                o.ModifiedOn = now;

                DefaultStorage.WeedDirCreate(o);

                data = DefaultStorage.WeedDirList(identity.Id);
            }
            var hierarchy = data.Select(x => new Node { Id = x.First.ToString(), Text = x.Third, Pid = x.Second.ToString() }).ToHierarchy(pid.ToString());
            model.Nodes = hierarchy;
            //var model = new ExploreModel();
            //if (!id.IsNullOrEmpty())
            //{
            //    var file = DefaultStorage.WeedFileGet(id);
            //    if (file != null)
            //    {
            //        did = file.Did;
            //    }
            //}
            //else
            //{
            //    // var dir=DefaultStorage.WeedDirGet()
            //}
            //if (identity)
            //{
            //    did = DefaultStorage.WeedDirExist(identity.Id);
            //    if (did == 0)
            //    {
            //        var o = new WeedDir();
            //        did = DefaultStorage.WeedDirCreate(o);
            //    }
            //    model.Files = DefaultStorage.WeedFilePagedList(identity.Id, did);
            //}
            return View(model);
        }



        [HttpGet("dir/{did}")]
        public IActionResult FileList(int did)
        {
            var identity = User;
            var model = DefaultStorage.WeedFilePagedList(identity.Id, did);
            return PartialView(model);
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="param"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("dir/{did}")]
        public async Task<ActionResult> Upload(int did, IFormFile file = null)
        {
            var identity = User;
            var result = new Result();
            var tpl = @"<script type='text/javascript'>window.parent.fileUploadCallback({0});</script>";
            if (did < 1)
            {
                result.Message = "参数有误";
                var d = JsonConvert.SerializeObject(result);
                var c = string.Format(tpl, d);
                return Content(c, "text/html");
            }
            if (file == null)
            {
                result.Message = "请选择文件";
                var d = JsonConvert.SerializeObject(result);
                var c = string.Format(tpl, d);
                return Content(c, "text/html");
            }

            if (!identity)
            {
                result.Message = "未登录或登录已超时";
                var d = JsonConvert.SerializeObject(result);
                var c = string.Format(tpl, d);
                return Content(c, "text/html");
            }
            var dir = DefaultStorage.WeedDirGet(did);
            if (dir == null || dir.UserId != identity.Id)
            {
                result.Message = "非法请求";
                var d = JsonConvert.SerializeObject(result);
                var c = string.Format(tpl, d);
                return Content(c, "text/html");
            }

            var name = file.FileName.ToLower();

            var r = await weedService.Upload(name, file.OpenReadStream(), identity.Id, did);
            if (r)
            {
                result.Status = true;
                result.Message = WebSite.Asset + "/" + r.Fid;

                var d = JsonConvert.SerializeObject(result);
                var c = string.Format(tpl, d);
                return Content(c, "text/html");
            }
            result.Message = r.Error;
            var data = JsonConvert.SerializeObject(result);
            var js = string.Format(tpl, data);
            return Content(js, "text/html");
        }

        private void OnUpload(UploadResult r)
        {
            if (r)
            {
                var identity = User;
                var now = DateTime.Now;
                var ext = Path.GetExtension(r.FileName);
                var file = new WeedFile();
                file.Id = r.Fid;
                file.Uid = r.Uid;
                file.Did = r.Did;
                file.Ext = ext;
                file.Url = r.FileUrl;
                file.Size = r.Size;
                file.Name = r.FileName;
                file.Badge = string.Empty;
                file.Extra = string.Empty;
                file.Status = true;
                file.CreatedBy = identity.Name;
                file.CreatedOn = now;
                file.ModifiedBy = identity.Name;
                file.ModifiedOn = now;

                var id = DefaultStorage.WeedFileCreate(file);
                if (id < 1)
                {
                    Logger.LogError("保存到数据库失败：" + r.Error);
                }
                return;
            }
            Logger.LogError("上传失败：" + r.Error);
        }
    }
}