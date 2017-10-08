using System;
using Ayatta.Domain;
using Newtonsoft.Json.Linq;

namespace Ayatta.OAuth
{
    /// <summary>
    /// 开心网
    /// </summary>
    internal sealed class KaixinAuthProvider : AuthProvider
    {
        public KaixinAuthProvider(OAuthProvider provider) : base(provider)
        {
        }

        protected override Result<UserOAuth> Callback(string content)
        {
            //https://api.kaixin001.com/oauth2/access_token
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
            user.AccessToken = accessToken;
            user.RefreshToken = refreshToken;
            user.ExpiredOn = DateTime.Now.AddSeconds(expiresIn);

            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    //https://api.kaixin001.com/users/me
                    // todo
                    /*
                    var request = new RestRequest(OAuth.UserInfoResource);
                    request.AddParameter(AccessTokenKey, accessToken);

                    content = Client.Execute(request).Content;

                    var data = JObject.Parse(content);
                    
                    if (data.TryGetValue(ErrorKey, out error))
                    {
                        result.Message = error.Value<string>();
                        return result;
                    }
                    user.OpenId = data["uid"].Value<string>();
                    user.Name = data["uname"].Value<string>();
                    */
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