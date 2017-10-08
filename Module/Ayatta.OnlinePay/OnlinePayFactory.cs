using System;
using Ayatta.Domain;

namespace Ayatta.OnlinePay
{
    public static class OnlinePayFactory
    {
        public static IOnlinePay Create(PaymentPlatform platform)
        {
            switch (platform.Id)
            {
                case 2:
                    return new AliPay(platform);
                case 3:
                    return new WeixinPay(platform);
                case 4:
                    return new AliAppPay(platform);
                case 5:
                    return new TenPay(platform);
                default: throw new NotSupportedException();
            }
        }
    }
}