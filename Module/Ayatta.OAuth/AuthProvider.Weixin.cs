using System;
using Ayatta.Domain;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Ayatta.OAuth
{
    /// <summary>
    /// 微信
    /// </summary>
    internal sealed class WeixinAuthProvider : AuthProvider
    {
        public WeixinAuthProvider(OAuthProvider provider) : base(provider)
        {
        }

        public override string GetLoginUri(string state = null)
        {
            var qs = QueryString.Create("response_type", "code")
                .Add("appid", Provider.ClientId)
                .Add("redirect_uri", Provider.CallbackEndpoint)
                .Add("scope", Provider.Scope)
                .Add("state", state ?? "")
                .ToString();

            return Provider.AuthorizationEndpoint + qs;
        }

        public override Result<UserOAuth> Callback(IQueryCollection param)
        {
            var error = param[ErrorKey];
            var result = new Result<UserOAuth>();
            if (!string.IsNullOrEmpty(error))
            {
                result.Message = error;
                return result;
            }
            var dic = new Dictionary<string, string>
            {
                {"code", param["code"]},
                {"appid", Provider.ClientId},
                {"secret", Provider.ClientSecret},
                {"grant_type", "authorization_code"}
            };

            var qs = QueryString.Create(dic).ToString();

            var content = Client.GetStringAsync(Provider.TokenEndpoint + qs).Result;

            result = Callback(content.ToString());

            if (result.Status)
            {
                result.Data.Provider = Name;
                result.Data.ModifiedBy = Name;
                result.Data.ModifiedOn = DateTime.Now;
                result.Data.Id = OnAuthorized(result.Data);
            }
            return result;
        }


        protected override Result<UserOAuth> Callback(string content)
        {
            var result = new Result<UserOAuth>();
            try
            {
                JToken error;
                var data = JObject.Parse(content);

                if (data.TryGetValue(ErrorKey, out error))
                {
                    result.Message = error.Value<string>();
                    return result;
                }

                var user = new UserOAuth();

                var accessToken = data[AccessTokenKey].Value<string>();
                var expiresIn = data[ExpiresInKey].Value<int>();

                user.AccessToken = accessToken;
                user.ExpiredOn = DateTime.Now.AddSeconds(expiresIn);
                user.RefreshToken = data[RefreshTokenKey].Value<string>();

                user.OpenId = data["unionid"].Value<string>();

                if (string.IsNullOrEmpty(user.OpenId))
                {
                    user.OpenId = data["openid"].Value<string>();                   
                }
                user.OpenName = Name;
                user.Scope = data[ScopeKey].Value<string>();
                

                result.Data = user;
                result.Status = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message + content;
            }
            return result;
        }
    }
}