using System;
using Ayatta.Domain;
using Newtonsoft.Json.Linq;

namespace Ayatta.OAuth
{
    /// <summary>
    /// 豆瓣
    /// </summary>
    internal sealed class DoubanAuthProvider : AuthProvider
    {
        public DoubanAuthProvider(OAuthProvider provider) : base(provider)
        {
        }
        protected override Result<UserOAuth> Callback(string content)
        {
            //https://www.douban.com/service/auth2/token
            //var content = response.Content;
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

                user.OpenId = data["douban_user_id"].Value<string>();
            }
            catch (Exception e)
            {
                result.Message = e.Message + content;
            }
            return result;
        }
    }
}