using System;
using System.Text;
using Ayatta.Storage;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public abstract class AbstractController : Controller
    {
        protected ILogger Logger { get; }
        protected IDistributedCache DefaultCache { get; }
        protected DefaultStorage DefaultStorage { get; }

        protected static readonly string PasswordSalt = "4EE48390-C779-4F8D-9423-F3449F6BEBE3";

        protected AbstractController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger logger)
        {
            if (defaultStorage == null)
            {
                throw new ArgumentNullException(nameof(defaultStorage));
            }
            DefaultStorage = defaultStorage;
            DefaultCache = defaultCache;
            Logger = logger;
        }

        public IActionResult Jsonp(object value, string callback = "callback")
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            var data = callback + "(" + json + ");";
            return Content(data, "application/javascript", Encoding.UTF8);
        }
        /*
        #region Captcha      

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="key">验证码key</param>
        /// <returns></returns>
        protected void RenderCaptcha(string key)
        {
            var captcha = new CaptchaImage();

            SetSession(key, captcha.Text.ToLower());

            using (var img = captcha.Render())
            {
                img.Save(Response.Body, ImageFormat.Png);
            }
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="captchaKey">验证码key</param>
        /// <param name="captchaText">验证码</param>
        /// <returns></returns>
        protected bool ValidateCaptcha(string captchaKey, string captchaText)
        {
            return GetStringSession(captchaKey) == captchaText.ToLower();
        }
        #endregion
        */
        #region Session


        protected void SetSession(string key, int value)
        {
            HttpContext.Session?.SetInt32(key, value);
        }

        protected int? GetIntSession(string key)
        {
            return HttpContext.Session?.GetInt32(key);
        }

        protected void SetSession(string key, string value)
        {
            if (value != null)
            {
                HttpContext.Session?.SetString(key, value);
            }

        }
        protected string GetStringSession(string key)
        {
            return HttpContext.Session?.GetString(key);
        }

        protected void RemoveSession(string key)
        {
            HttpContext.Session?.Remove(key);
        }
        #endregion
    }
}
