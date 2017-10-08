using Ayatta.Nsq;
using Ayatta.Storage;
using Ayatta.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("security")]

    public class SecurityController : BaseController
    {
        private readonly INsqService nsqService;
        public SecurityController(INsqService nsqService, DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<SecurityController> logger) : base(defaultStorage, defaultCache, logger)
        {
            this.nsqService = nsqService;
        }

        #region 密码更新

        /// <summary>
        /// 密码更新
        /// </summary>            
        /// <returns></returns>
        [HttpGet("pwd-update")]
        public IActionResult PwdUpdate()
        {
            return View();
        }

        /// <summary>
        ///  密码更新
        /// </summary>
        /// <param name="old">原密码</param>            
        /// <param name="pwd">新密码</param>            
        /// <returns></returns>
        [HttpPost("pwd-update")]
        public IActionResult PwdUpdate(string old,string pwd)
        {
            var identity = User;
            var result = new Result();
            if (!identity)
            {
                result.Message = "请先登录！";
                return Json(result);
            }
            if (!old.IsNullOrEmpty())
            {
                result.Message = "请输入原密码！";
                return Json(result);
            }
            if (!pwd.IsNullOrEmpty())
            {
                result.Message = "请输入新密码！";
                return Json(result);
            }
            if (!pwd.IsPassword())
            {
                result.Message = "6-18个字符（至少包含两个字母、两个数字、一个特殊字符）！";
                return Json(result);
            }
            if (old == pwd)
            {
                result.Message = "新密码不能与原密码相同！";
                return Json(result);
            }
            var x = (old + PasswordSalt).Hash();
            var u = DefaultStorage.UserGet(identity.Name, pwd);
            if (u == null)
            {
                result.Message = "原密码不正确！";
                return Json(result);
            }
            var y  = (pwd + PasswordSalt).Hash();
            result.Status = DefaultStorage.UserPasswordUpdate(identity.Id, y);
            if (!result.Status)
            {
                result.Message = "更新密码失败！";
            }            
            return Json(result);
        }
        #endregion

        

        /// <summary>
        /// 第三方帐号绑定
        /// </summary>            
        /// <returns></returns>
        [HttpGet("/security/test")]
        public IActionResult Test()
        {
           var id= nsqService.Publish("xxx", new TestMessage() { Name = "test" });
            return Content(id);
        }


        /// <summary>
        /// 第三方帐号绑定
        /// </summary>            
        /// <returns></returns>
        [HttpGet("/security/bind")]
        public IActionResult Bind()
        {
            return View();
        }

        /// <summary>
        /// 第三方帐号绑定
        /// </summary>            
        /// <returns></returns>
        [HttpPost("/security/bind")]
        public IActionResult Bind(string uid, string captcha)
        {
            return View();
        }
    }
}