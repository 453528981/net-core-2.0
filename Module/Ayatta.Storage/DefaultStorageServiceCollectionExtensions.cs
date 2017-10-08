using System;
using Ayatta.Storage;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up Storage related services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class DefaultStorageServiceCollectionExtensions
    {
        /// <summary>
        /// Adds DefaultStorage services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddDefaultStorage(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<DefaultStorage, DefaultStorage>();

            return services;
        }
        /// <summary>
        /// Adds DefaultStorage services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{StorageOptions}"/> to configure the provided <see cref="DefaultStorage"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddDefaultStorage(this IServiceCollection services, Action<StorageOptions> setupAction)
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
            services.AddSingleton<DefaultStorage, DefaultStorage>();

            return services;
        }
    }
}