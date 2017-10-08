using System;
using Ayatta.Domain;

namespace Ayatta.OAuth
{
    public static class AuthProviderFactory
    {
        public static IAuthProvider Create(OAuthProvider provider)
        {
            switch (provider.Id.ToLower())
            {
                case "qq":
                    return new QqAuthProvider(provider);
                case "weixin":
                    return new WeixinAuthProvider(provider);
                case "sina":
                    return new SinaAuthProvider(provider);
                default: throw new NotSupportedException();
            }
        }
    }
}