using System;
using Ayatta.Domain;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace Ayatta.OAuth
{
    /// <summary>
    /// 奇虎360
    /// </summary>
    internal sealed class QhAuthProvider : AuthProvider
    {
        public QhAuthProvider(OAuthProvider provider) : base(provider)
        {
        }

        protected override Result<UserOAuth> Callback(string content)
        {
            //https://openapi.360.cn/oauth2/access_token
            //var content = response.Content;
            var result = new Result<UserOAuth>();

            JToken error;
            var param = JObject.Parse(content);

            if (param.TryGetValue(ErrorKey, out error))
            {
                result.Message = error.Value<string>();
                return result;
            }
            var user = new UserOAuth();
            var accessToken = param[AccessTokenKey].Value<string>();
            var refreshToken = param[RefreshTokenKey].Value<string>();
            var expiresIn = param[ExpiresInKey].Value<int>();
            var scope = param[ScopeKey].Value<string>();

            user.Scope = scope;
            user.AccessToken = accessToken;
            user.RefreshToken = refreshToken;
            user.ExpiredOn = DateTime.Now.AddSeconds(expiresIn);

            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    //https://openapi.360.cn/user/me.json

                    var qs = QueryString.Create(AccessTokenKey, accessToken).ToString();

                    content = Client.GetStringAsync(Provider.UserEndpoint + qs).Result;

                    var data = JObject.Parse(content);

                    if (data.TryGetValue(ErrorKey, out error))
                    {
                        result.Message = error.Value<string>();
                        return result;
                    }
                    user.OpenId = data["id"].Value<string>();
                    user.OpenName = data["name"].Value<string>();

                    result.Data = user;
                    result.Status = true;
                }
                catch (Exception e)
                {
                    result.Message = e.Message + content;
                }
            }
            return result;
        }
    }
}