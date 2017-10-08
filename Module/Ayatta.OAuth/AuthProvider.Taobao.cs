using System;
using Ayatta.Domain;
using Newtonsoft.Json.Linq;

namespace Ayatta.OAuth
{
    /// <summary>
    /// 淘宝
    /// </summary>
    internal sealed class TaobaoAuthProvider : AuthProvider
    {
        public TaobaoAuthProvider(OAuthProvider provider) : base(provider)
        {
        }

        protected override Result<UserOAuth> Callback(string content)
        {
            // https://oauth.taobao.com/token
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

                user.OpenId = data["taobao_user_id"].Value<string>();
                user.OpenName = data["taobao_user_nick"].Value<string>();

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