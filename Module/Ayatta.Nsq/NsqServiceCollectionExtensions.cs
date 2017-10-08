using System;
using Ayatta.Nsq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NsqServiceCollectionExtensions
    {
        public static IServiceCollection AddNsq(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            return services.AddSingleton<INsqService, NsqService>();
        }

        public static IServiceCollection AddNsq(this IServiceCollection services, Action<NsqOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            return services.AddOptions().Configure(setupAction).AddSingleton<INsqService, NsqService>();
        }
    }
}
