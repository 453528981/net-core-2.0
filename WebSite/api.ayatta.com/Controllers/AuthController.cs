using Ayatta.Storage;
using Ayatta.Message;
using Ayatta.Service;
using WebApp.Parameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace WebApp.Controllers
{
    public class AuthController : BaseController
    {

        private readonly ISmsService smsService;
        public AuthController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<AuthController> logger) : base(defaultStorage, defaultCache, logger)
        {
            //this.smsService = smsService;
        }
        
               [HttpPost("auth")]
               public SmsSendResponse UserGet([FromBody]SmsSendRequest req)
               {
                   var rep = new SmsSendResponse();
                   var message = new SmsMessage();
                   message.Mobile = req.Mobile;
                   message.Content = "abc";//¶ÌÐÅÑéÖ¤Âë
                   var status = smsService.Send(message);
                   if (status)
                   {
                       rep.Data = message.Content;
                   }
                   return rep;
               }
               
    }
}