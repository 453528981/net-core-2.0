using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ayatta.Web
{
    public class Startup
    {
        protected IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<StorageOptions>(Configuration.GetSection("ConnectionStrings"));


            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie.Name = "x-xsrf-token";
                options.FormFieldName = "x-xsrf-token";
            });

            services.AddDistributedMemoryCache();

            services.AddDefaultStorage();

            //services.AddWeed();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
               

            services.AddSession(options =>
            {
                options.Cookie.Name = "x-session";
            });
            services.AddCart();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }
    }
}