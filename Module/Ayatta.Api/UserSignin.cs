
using Ayatta.Domain;

namespace Ayatta.Api
{
    public sealed class UserSignInResponse : Response
    {
        public User Data { get; set; }
    }
    public sealed class UserSignInRequest : Request<UserSignInResponse>
    {
        public string Uid { get; set; }
        public string Pwd { get; set; }
        public bool IsOAuth { get; set; }
        public AuthParam OAuthParam { get; set; }

        public string Ip { get; set; }
        public string TraceCode { get; set; }


        public class AuthParam
        {
            public string Provider { get; set; }
            public string OpenId { get; set; }
            public string OpenName { get; set; }
            public string Scope { get; set; }
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public int Expire { get; set; }
        }
    }
}