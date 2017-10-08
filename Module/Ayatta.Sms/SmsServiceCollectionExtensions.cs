using System;
using Ayatta.Sms;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SmsServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsService(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton<ISmsService, SmsService>();

            return services;
        }

        public static IServiceCollection AddSmsService(this IServiceCollection services, Action<SmsOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();
            services.Configure(setupAction);
            services.AddSingleton<ISmsService, SmsService>();

            return services;
        }
    }
}
