using System;
using Ayatta.Sms;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using Ayatta.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("security")]
    public class SecurityController : BaseController
    {
        private readonly ISmsService smsService;

        public SecurityController(ISmsService smsService, DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<SecurityController> logger) : base(defaultStorage, defaultCache, logger)
        {
            this.smsService = smsService;
        }

        /// <summary>
        /// 安全中心
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var identity = User;
            var model = new SecurityIndexModel();
            model.User = DefaultStorage.UserGet(identity.Id);
            return View(model);
        }

        #region 密码更新
        [HttpGet("password")]
        public IActionResult Password()
        {
            return View();
        }

        [HttpPost("password")]
        public IActionResult Password(string oldPwd, string newPwd)
        {
            var result = new Result();
            var identity = User;
            if (!identity)
            {
                result.Message = "未登录或登录超时！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(oldPwd))
            {
                result.Message = "请输入原密码！";
                return Json(result);
            }
            if (!newPwd.IsPassword())
            {
                result.Message = "密码为6-12位，至少两个字母、两个数字、一个特殊字符！";
                return Json(result);
            }

            var user = DefaultStorage.UserGet(identity.Id);
            if (user == null)
            {
                result.Message = "用户不存在！";
                return Json(result);
            }

            if (oldPwd.Hash() != user.Password)
            {
                result.Message = "请输入正确的原密码！";
                return Json(result);
            }
            var pwd = (newPwd + PasswordSalt).Hash();

            if (user.Password == pwd)
            {
                result.Message = "新密码不能与原密码一样！";
                return Json(result);
            }
            var status = DefaultStorage.UserPasswordUpdate(identity.Id, pwd);
            if (!result.Status)
            {
                result.Message = "更新密码失败！";
                return Json(result);
            }
            result.Status = true;
            return Json(result);
        }

        #endregion

        #region 手机绑定
        /// <summary>
        /// 手机绑定
        /// </summary>
        /// <returns></returns>
        [HttpGet("mobile-bind")]

        public IActionResult MobileBind()
        {
            return View();
        }

        /// <summary>
        /// 手机绑定
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="guid">短信验证Id</param>
        /// <param name="captcha">短信验证码 </param>
        /// <returns></returns>
        [HttpPost("mobile-bind")]
        public IActionResult MobileBind(string mobile, string guid, string captcha)
        {
            var result = new Result();
            var identity = User;
            if (!identity)
            {
                result.Message = "未登录或登录超时！";
                return Json(result);
            }
            if (!mobile.IsMobile())
            {
                result.Message = "请输入正确的手机号码！";
                return Json(result);
            }

            var status = ValidateSmsCaptcha(mobile, guid, captcha);
            if (!status)
            {
                result.Message = "短信验证码不正确或已过期！";
                return Json(result);
            }
            var exist = DefaultStorage.UserExist(mobile);
            if (exist)
            {
                result.Message = "该手机号码已绑定其它帐号，请换个手机号码！";
                return Json(result);
            }
            status = DefaultStorage.UserMobileUpdate(identity.Id, mobile);
            if (!status)
            {
                result.Message = "绑定手机失败！";
                return Json(result);
            }
            result.Status = true;
            return Json(result);
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost("sms-captcha-send")]
        public async Task<IActionResult> SmsSend(string mobile)
        {
            var result = new Result();
            var identity = User;
            if (!identity)
            {
                result.Message = "未登录或登录超时！";
                return Json(result);
            }
            if (!mobile.IsMobile())
            {
                result.Message = "请输入正确的手机号码！";
                return Json(result);
            }
            var exist = DefaultStorage.UserExist(mobile);
            if (exist)
            {
                result.Message = "该手机号码已绑定其它帐号，请换个手机号码！";
                return Json(result);
            }

            var captcha = GenerateCaptcha();
            var message = "手机绑定验证码 " + captcha;

            var status = await smsService.SendMessage(mobile, "手机绑定验证码", message);
            if (!status)
            {
                result.Message = "发送短信失败，请稍后再试！";
                return Json(result);
            }

            result.Status = true;            
            result.Message = status.Guid;

            var key = $"{mobile}-{status.Guid}";
            DefaultCache.SetString(key, captcha, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = new TimeSpan(0, 2, 0) });

            return Json(result);
        }

        /// <summary>
        /// 生成短信验证码
        /// </summary>
        /// <returns></returns>
        private static string GenerateCaptcha()
        {
            string str = "";
            var random = new Random();
            for (int i = 0; i < 6; i++)
            {
                str += random.Next(1, 10);
            }
            return str;
        }

        /// <summary>
        /// 校验短信验证码是否有效
        /// </summary>
        /// <param name="guid">guid</param>
        /// <param name="mobile">手机号</param>
        /// <param name="captcha">验证码</param>
        /// <returns></returns>
        private bool ValidateSmsCaptcha(string guid, string mobile, string captcha)
        {
            if (string.IsNullOrEmpty(guid) || string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(captcha))
            {
                return false;
            }
            var key = $"{mobile}-{guid}";
            return DefaultCache.GetString(key) == captcha;
        }
        #endregion
    }
}
