using System;
using Ayatta.Api;
using Ayatta.Sms;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public class SmsController : BaseController
    {
        private readonly ISmsService smsService;
        public SmsController(ISmsService smsService, DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<SmsController> logger) : base(defaultStorage, defaultCache, logger)
        {
            this.smsService = smsService;
        }

        [HttpPost("sms-captcha-send")]
        public async Task<SmsSendResponse> SmsSend([FromBody]SmsSendRequest req)
        {
            var rep = new SmsSendResponse();

            var captcha = GenerateCaptcha();
            var message = "²âÊÔ ×¢²áÑéÖ¤Âë " + captcha;

            var result = await smsService.SendMessage(req.Mobile, "×¢²áÑéÖ¤Âë", message);
            if (result)
            {
                rep.Data = captcha;
                rep.Guid = result.Guid;
               
                var key = $"{req.Mobile}-{result.Guid}";
                DefaultCache.SetString(key, captcha, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = new TimeSpan(0, 2, 0) });
            }
            else
            {
                rep.Error(result.Message);
                return rep;
            }
            return rep;
        }

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

    }
}