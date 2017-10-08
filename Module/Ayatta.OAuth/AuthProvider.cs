using System;
using Ayatta.Domain;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Ayatta.OAuth
{
    internal abstract class AuthProvider : IAuthProvider
    {
        protected const string AccessTokenKey = "access_token";
        protected const string RefreshTokenKey = "refresh_token";
        protected const string ExpiresInKey = "expires_in";
        protected const string ScopeKey = "scope";
        protected const string ErrorKey = "error";

        protected readonly HttpClient Client;
        protected readonly OAuthProvider Provider;

        public string State { get; private set; }

        public string Name => Provider.Id;

        public Func<UserOAuth, int> Authorized { get; set; }

        protected AuthProvider(OAuthProvider provider)
        {
            this.Provider = provider;
            Client = new HttpClient { Timeout = new TimeSpan(0, 0, 30) };
        }

        /// <summary>
        /// 用户在第三方平台登录成功并授权后触发事件
        /// </summary>
        /// <param name="o"></param>
        protected virtual int OnAuthorized(UserOAuth o)
        {
            var handler = Authorized;
            return handler?.Invoke(o) ?? 0;
        }

        public virtual string GetLoginUri(string state = null)
        {
            var qs = QueryString.Create("response_type", "code")
                .Add("client_id", Provider.ClientId)
                .Add("redirect_uri", Provider.CallbackEndpoint)
                .Add("scope", Provider.Scope)
                .Add("state", state ?? "")
                .ToString();

            return Provider.AuthorizationEndpoint + qs;
        }

        public virtual Result<UserOAuth> Callback(IQueryCollection param)
        {
            State = param["state"];
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
                {"client_id", Provider.ClientId},
                {"client_secret", Provider.ClientSecret},
                {"redirect_uri", Provider.CallbackEndpoint},
                {"grant_type", "authorization_code"}
            };

            var hc = new FormUrlEncodedContent(dic);
            var content = Client.PostAsync(Provider.TokenEndpoint, hc).Result.Content;

            result = Callback(content.ToString());

            if (result.Status)
            {
                result.Data.Provider = Name;
                result.Data.CreatedOn = DateTime.Now;
                if (string.IsNullOrEmpty(result.Data.OpenName))
                {
                    result.Data.OpenName = Name;
                }
                result.Data.Id = OnAuthorized(result.Data);
            }
            return result;
        }

        protected abstract Result<UserOAuth> Callback(string content);

    }
}