using System;
using Ayatta.Weed;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WeedServiceCollectionExtensions
    {
        public static IServiceCollection AddWeed(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton<IWeedService, WeedService>();

            return services;
        }

        public static IServiceCollection AddWeed(this IServiceCollection services, Action<WeedOptions> setupAction)
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
            services.AddSingleton<IWeedService, WeedService>();

            return services;
        }
    }
}
