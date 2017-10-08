using System;
using Ayatta.Domain;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
namespace Ayatta.OAuth
{
    /// <summary>
    /// QQ
    /// </summary>
    internal sealed class QqAuthProvider : AuthProvider
    {
        public QqAuthProvider(OAuthProvider provider) : base(provider)
        {
        }

        protected override Result<UserOAuth> Callback(string content)
        {
            //https://graph.qq.com/oauth2.0/token
            //var content = response.Content;
            var result = new Result<UserOAuth>();
            if (content.Contains("callback"))
            {
                var lpos = content.IndexOf('(');
                var rpos = content.IndexOf(')');
                content = content.Substring(lpos + 1, rpos - lpos - 1);

                JToken error;
                var data = JObject.Parse(content);

                if (data.TryGetValue(ErrorKey, out error))
                {
                    result.Message = error.Value<string>();
                    return result;
                }
            }

            var param = QueryHelpers.ParseQuery(content);

            var user = new UserOAuth();
            var accessToken = param[AccessTokenKey];
            var expiresIn = Convert.ToInt32(param[ExpiresInKey]);

            user.AccessToken = accessToken;
            user.RefreshToken = param[RefreshTokenKey];
            user.ExpiredOn = DateTime.Now.AddSeconds(expiresIn);

            if (!string.IsNullOrEmpty(accessToken))
            {
                var qs = QueryString.Create(AccessTokenKey, accessToken).ToString();

                content = Client.GetStringAsync(Provider.UserEndpoint + qs).Result;
                /*
                var request = new RestRequest(OAuth.UserInfoResource);
                request.AddParameter(AccessTokenKey, accessToken);
                content = Client.Execute(request).Content;

                var lpos = content.IndexOf('(');
                var rpos = content.IndexOf(')');
                content = content.Substring(lpos + 1, rpos - lpos - 1);
                */
                try
                {
                    JToken error;
                    var data = JObject.Parse(content);

                    if (data.TryGetValue(ErrorKey, out error))
                    {
                        result.Message = error.Value<string>();
                        return result;
                    }

                    user.OpenId = data["openid"].Value<string>();

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