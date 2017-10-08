using System;
using Ayatta.Domain;
using Newtonsoft.Json.Linq;

namespace Ayatta.OAuth
{
    /// <summary>
    /// 百度
    /// </summary>
    internal sealed class BaiduAuthProvider : AuthProvider
    {
        public BaiduAuthProvider(OAuthProvider provider): base(provider)
        {
        }

        protected override Result<UserOAuth> Callback(string content)
        {
            //https://openapi.baidu.com/rest/2.0/passport/users/getLoggedInUser

            
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
            var expiresIn = param[ExpiresInKey].Value<int>();
            user.AccessToken = accessToken;
            user.ExpiredOn = DateTime.Now.AddSeconds(expiresIn);

            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
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