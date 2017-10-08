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
    [Route("misc")]
    public class MiscController : BaseController
    {
        private readonly ISmsService smsService;
        public MiscController(ISmsService smsService, DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<MiscController> logger) : base(defaultStorage, defaultCache, logger)
        {
            this.smsService = smsService;
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
            var message = "注册验证码 " + captcha;

            var status = await smsService.SendMessage(mobile, message, "注册验证码" );
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

        [HttpGet("cache/{key}")]
        public IActionResult SendMobileCaptcha(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Content("请输入key");
            }
            var val = DefaultCache.GetString(key);
            
            return Content(val);
        }

        //[HttpPost("/misc/mobile-captcha-send/0")]
        //public IActionResult SendMobileCaptcha(string mobile)
        //{
        //    var result = new Result<string>();
        //    result.Status = true;
        //    return Json(result);
        //}
    }
}