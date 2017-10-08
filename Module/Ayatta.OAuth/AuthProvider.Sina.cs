using System;
using Ayatta.Domain;
using Newtonsoft.Json.Linq;

namespace Ayatta.OAuth
{
    /// <summary>
    /// 新浪
    /// </summary>
    internal sealed class SinaAuthProvider : AuthProvider
    {
        public SinaAuthProvider(OAuthProvider provider) : base(provider)
        {
        }

        protected override Result<UserOAuth> Callback(string content)
        {
            //https://api.weibo.com/oauth2/access_token

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

                user.OpenId = data["uid"].Value<string>();

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