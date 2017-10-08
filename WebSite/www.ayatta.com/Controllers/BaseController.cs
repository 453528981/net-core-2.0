using System;
using System.Linq;
using Ayatta.OAuth;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Extensions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ayatta.Web.Controllers
{

    public abstract class BaseController : AbstractController
    {
        public new Identity User
        {
            get { return base.User.Identity(); }
        }
        protected BaseController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        #region SignIn/SignOut
        protected void SignIn(User user)
        {

            var ci = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            ci.AddClaim(new Claim("id", user.Id.ToString()));
            ci.AddClaim(new Claim("guid", user.Guid));

            var cp = new ClaimsPrincipal(ci);
          
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cp);
        }

        protected void SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        #endregion

        #region OAuthProvider
        protected IAuthProvider GetAuthProvider(string id)
        {
            var providers = DefaultStorage.OAuthProviderList();
            var provider = providers.FirstOrDefault(x => x.Id == id);

            if (provider == null) return null;

            var p = AuthProviderFactory.Create(provider);
            if (p != null)
            {
                p.Authorized += Authorized;
            }
            return p;
        }

        /// <summary>
        /// 用户在第三方平台登录成功并授权后触发
        /// 生成系统帐号
        /// </summary>
        /// <param name="o"></param>
        /// <returns>用户Id</returns>
        private int Authorized(UserOAuth o)
        {
            int userId;
            var openUser = DefaultStorage.UserOAuthGet(o.Provider, o.OpenId);
            if (openUser != null)
            {
                userId = openUser.Id;
                Task.Factory.StartNew(() =>
                {
                    o.Id = userId;
                    o.CreatedOn = DateTime.Now;

                    try
                    {
                        DefaultStorage.UserOAuthUpdate(o);
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(1, "OAuth {0} Authorized UserOAuthUpdate 失败 UserId({1}} {2}", o.Provider, userId, e.Message);
                    }
                });

            }
            else
            {
                userId = DefaultStorage.UserIdGet(o.OpenId, o.Provider);
                if (userId == 0)
                {
                    var user = new User();
                    var profile = new UserProfile();

                    var now = DateTime.Now;

                    user.Guid = Guid.NewGuid().ToString("N");
                    user.Name = o.OpenId;
                    user.Email = string.Empty;
                    user.Mobile = string.Empty;
                    user.Nickname = o.OpenName ?? "";
                    user.Password = string.Empty;
                    user.Role = UserRole.Buyer;
                    user.Grade = UserGrade.One;
                    user.Limitation = UserLimitation.None;
                    user.Permission = UserPermission.None;
                    user.Avatar = string.Empty;
                    user.Status = UserStatus.Normal;
                    user.AuthedOn = null;
                    user.CreatedBy = o.Provider;
                    user.CreatedOn = now;
                    user.ModifiedBy = "";
                    user.ModifiedOn = now;

                    profile.Code = string.Empty;
                    profile.Name = string.Empty;
                    profile.Gender = Gender.Secrect;
                    profile.Marital = Marital.Secrect;
                    profile.Birthday = null;
                    profile.Phone = string.Empty;
                    profile.Mobile = string.Empty;
                    profile.RegionId = string.Empty;
                    profile.Street = string.Empty;
                    profile.SignUpIp = "";
                    profile.SignUpBy = 0;
                    profile.TraceCode = "";
                    profile.LastSignInIp = "";
                    profile.LastSignInOn = now;

                    user.Profile = profile;

                    userId = DefaultStorage.UserCreate(user);
                }
                if (userId > 0)
                {
                    Task.Factory.StartNew(() =>
                    {
                        o.Id = userId;
                        o.CreatedOn = DateTime.Now;
                        try
                        {
                            DefaultStorage.UserOAuthCreate(o);
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(1, "OAuth {0} Authorized UserOAuthCreate 失败 UserId({1}} {2}", o.Provider, userId, e.Message);
                        }
                    });
                }
            }
            return userId;
        }
        #endregion

        protected IList<PaymentPlatform> PaymentPlatformList()
        {
            var key = "payment-platmfors";
            return DefaultCache.Put(key, () => PaymentPlatformList(true), DateTime.Now.AddDays(1));
        }

        private IList<PaymentPlatform> PaymentPlatformList(bool status)
        {
            var platforms = DefaultStorage.PaymentPlatformList();
            foreach (var o in platforms)
            {
                o.Banks = DefaultStorage.PaymentBankList(o.Id);
            }
            return platforms;

        }

        /// <summary>
        /// 生成短信验证码
        /// </summary>
        /// <returns></returns>
        protected static string GenerateCaptcha()
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
        protected bool ValidateSmsCaptcha(string guid, string mobile, string captcha)
        {
            if (string.IsNullOrEmpty(guid) || string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(captcha))
            {
                return false;
            }
            var key = $"{mobile}-{guid}";
            return DefaultCache.GetString(key) == captcha;
        }


    }

}